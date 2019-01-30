using System;
using System.Collections.Generic;
using System.Numerics;

namespace MAVLink.NET
{
    sealed class MAVLinkManager
    {
        /*
         * Multithreaded Singleton (https://docs.microsoft.com/en-us/previous-versions/msp-n-p/ff650316(v=pandp.10)
         */
        private static volatile MAVLinkManager instance;
        private static object syncRoot = new object();

        private MAVLinkManager() { }

        public static MAVLinkManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new MAVLinkManager();
                    }
                }
                return instance;
            }
        }

        private List<MAVLinkNode> MAVLinkNodes = new List<MAVLinkNode>();

        private static readonly double Lat2LonScale = 1.2493498129938325118272112550467;
        private static readonly double Lon2LatScale = 0.8004163362410785091197462331483;

        private static readonly double Degree2Radian = (Math.PI / 180);

        private static readonly float World2Local = 100 * 1000;
        
        public enum FORMATION
        {
            TRIANGLE    = 0,
            ROW         = 1,
            COLUMN      = 2,
            FLOCKING    = 3
        }

        public FORMATION FormationMode = FORMATION.TRIANGLE;

        /*
        ~MAVLinkManager()
        {
            foreach (MAVLinkNode node in MAVLinkNodes)
                node.Serial.Close();
        }
        */

        public MAVLinkNode RegisterAgent(string port, int baud)
        {
            MAVLinkNode node = new MAVLinkNode(port, baud);
            MAVLinkNodes.Add(node);

            return node;
        }

        public void RegisterAgent(MAVLinkNode node)
        {
            MAVLinkNodes.Add(node);
        }

        public MAVLinkNode FindNodeByPortName(string portName)
        {
            foreach (MAVLinkNode mavlinkNode in MAVLinkNodes)
                if (mavlinkNode.PortName.Equals(portName))
                    return mavlinkNode;
            return null;
        }

        /****************************************
         * Serial
         ****************************************/
        public void Open(string portName)
        {
            MAVLinkNode mavlinkNode = FindNodeByPortName(portName);
            if (mavlinkNode == null) return;

            try
            {
                if (!mavlinkNode.Serial.IsOpen)
                {
                    mavlinkNode.Serial.Open();
                    mavlinkNode.heartbeatWorker.RunWorkerAsync();
                }
            }
            catch (System.IO.IOException e)
            {
                Console.Error.WriteLine(e.Message);
                return;
            }
            catch (NullReferenceException e)
            {
                Console.Error.WriteLine(e.Message);
            }
        }

        public void Close(int index=0)
        {
            MAVLinkNode node = MAVLinkNodes[index];

            node.heartbeatWorker.Dispose();

            try
            {
                if (node.Serial.IsOpen)
                    node.Serial.Close();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
            }
        }

        public void CloseAll()
        {
            foreach (MAVLinkNode node in MAVLinkNodes)
                node.Serial.Close();
        }

        /**
         * 
         */
        public void IsLeader(int index=0)
        {
            foreach (MAVLinkNode node in MAVLinkNodes)
                node.IsLeader = false;
            MAVLinkNodes[index].IsLeader = true;
        }

        /****************************************
         * Reynold
         ****************************************/
        public static Vector3 Alignment(List<MAVLinkNode> nodes)
        {
            float x = nodes[0].Direction.X;
            float y = nodes[0].Direction.Y;
            return new Vector3(x, y, 0f);
        }

        public static Vector3 Separate(Vector3 self, Vector3 other1, Vector3 other2)
        {
            Vector3 vector1 = (self - other1);
            Vector3 vector2 = (self - other2);
            float x = vector1.Length();
            float y = vector2.Length();
            float kValue1 = x / (x + y);
            float kValue2 = y / (x + y);
            vector1 *= kValue2;
            vector2 *= kValue1;
            return vector1 + vector2;
        }

        public static Vector3 Cohesion(Vector3 position, Vector3 center)
        {
            return center - position;
        }

        /****************************************
         * Formation
         ****************************************/
        public void Flocking()
        {
            float kAlignment   = 1f;
            float kSeperation  = 1f;
            float kCohesion    = 1f;

            Vector3 flockCenter = new Vector3();
            for (int i = 0; i < MAVLinkNodes.Count; i++)
                flockCenter += MAVLinkNodes[i].Position;
            flockCenter /= MAVLinkNodes.Count;

            float[,] distance = new float[3, 3];
            for (int i = 0; i < MAVLinkNodes.Count; i++) for (int j = 0; j < MAVLinkNodes.Count; j++)
                {
                    distance[i, j] = (MAVLinkNodes[i].Position * World2Local - MAVLinkNodes[j].Position * World2Local).Length();
                    Console.WriteLine("distance[{0:d}][{1:d}]: {2:f6}", i, j, distance[i, j]);
                }

            Vector3 aVector2 = Vector3.Normalize(Alignment(MAVLinkNodes));
            Vector3 sVector2 = Vector3.Normalize(Separate(MAVLinkNodes[1].Position, MAVLinkNodes[0].Position, MAVLinkNodes[2].Position));
            Vector3 cVector2 = Vector3.Normalize(Cohesion(MAVLinkNodes[1].Position, flockCenter));

            Vector3 aVector3 = Vector3.Normalize(Alignment(MAVLinkNodes));
            Vector3 sVector3 = Vector3.Normalize(Separate(MAVLinkNodes[2].Position, MAVLinkNodes[0].Position, MAVLinkNodes[1].Position));
            Vector3 cVector3 = Vector3.Normalize(Cohesion(MAVLinkNodes[2].Position, flockCenter));

            if (distance[1, 2] < 0.25 || distance[1, 0] < 0.25)
            {
                kAlignment = 0.2f;
                kSeperation = 1;
                kCohesion = 0;

                aVector2 *= kAlignment;
                sVector2 *= kSeperation;
                cVector2 *= kCohesion;

                MAVLinkNodes[1].Direction = Vector3.Normalize(MAVLinkNodes[1].Position + (aVector2 + sVector2 + cVector2)) / World2Local;
            }
            else if ((distance[1, 0] >= 0.25 && distance[1, 0] < 0.5) && (distance[1, 2] >= 0.25 && distance[1, 2] < 0.5))
            {
                switch (FormationMode)
                {
                    case FORMATION.ROW:
                        Row(MAVLinkNodes);
                        break;
                    case FORMATION.COLUMN:
                        Column(MAVLinkNodes);
                        break;
                    case FORMATION.TRIANGLE:
                    default:
                        Triangle(MAVLinkNodes);
                        break;
                }
            }
            else
            {
                switch (FormationMode)
                {
                    case FORMATION.ROW:
                        Row(MAVLinkNodes);
                        break;
                    case FORMATION.COLUMN:
                        Column(MAVLinkNodes);
                        break;
                    case FORMATION.TRIANGLE:
                    default:
                        Triangle(MAVLinkNodes);
                        break;
                }
            }

            if (distance[1, 2] < 0.25 || distance[2, 0] < 0.25)
            {
                kAlignment = 0.2f;
                kSeperation = 1;
                kCohesion = 0;

                aVector3 *= kAlignment;
                sVector3 *= kSeperation;
                cVector3 *= kCohesion;

                MAVLinkNodes[2].Direction = Vector3.Normalize(MAVLinkNodes[2].Position + (aVector3 + sVector3 + cVector3)) / World2Local;
            }
            else if ((distance[1, 2] >= 0.25 && distance[1, 2] < 0.5) && (distance[2, 0] >= 0.25 && distance[2, 0] < 0.5))
            {
                switch (FormationMode)
                {
                    case FORMATION.ROW:
                        Row(MAVLinkNodes);
                        break;
                    case FORMATION.COLUMN:
                        Column(MAVLinkNodes);
                        break;
                    case FORMATION.TRIANGLE:
                    default:
                        Triangle(MAVLinkNodes);
                        break;
                }
            }
            else
            {
                switch (FormationMode)
                {
                    case FORMATION.ROW:
                        Row(MAVLinkNodes);
                        break;
                    case FORMATION.COLUMN:
                        Column(MAVLinkNodes);
                        break;
                    case FORMATION.TRIANGLE:
                    default:
                        Triangle(MAVLinkNodes);
                        break;
                }
            }

            Console.WriteLine("Node[0]: {0:f6}, {1:f6}", MAVLinkNodes[0].Position.X, MAVLinkNodes[0].Position.Y);
            Console.WriteLine("Node[1]: {0:f6}, {1:f6}", MAVLinkNodes[1].Direction.X, MAVLinkNodes[1].Direction.Y);
            Console.WriteLine("Node[2]: {0:f6}, {1:f6}", MAVLinkNodes[2].Direction.X, MAVLinkNodes[2].Direction.Y);

            MAVLinkNodes[1].NextWP(MAVLinkNodes[1].Direction.X, MAVLinkNodes[1].Direction.Y);
            MAVLinkNodes[2].NextWP(MAVLinkNodes[2].Direction.X, MAVLinkNodes[2].Direction.Y);
        }

        public static void Row(List<MAVLinkNode> nodes)
        {
            float scale = 0.00001f;
            
            Vector3 fVector = Vector3.Normalize(nodes[0].Position);
            Vector3 bVector = fVector * -1;

            bVector *= (scale * 2);

            Vector3 leaderPosition = nodes[0].Position;
            nodes[1].Direction = leaderPosition + bVector;
            nodes[2].Direction = leaderPosition + (bVector * 3);
        }

        public static void Column(List<MAVLinkNode> nodes)
        {
            float scale = 0.00001f;
            
            Vector3 fVector = Vector3.Normalize(nodes[0].Position);

            float x = (float) (Math.Cos(-90 * Degree2Radian) * fVector.X - Math.Sin(-90 * Degree2Radian) * fVector.Y);
            float y = (float) (Math.Sin(-90 * Degree2Radian) * fVector.X + Math.Cos(-90 * Degree2Radian) * fVector.Y);
            Vector3 rVector = Vector3.Normalize(new Vector3(x, y, 0f));

            Vector3 lVector = rVector * -1;
            
            rVector *= (scale * 4.5f);
            lVector *= (scale * 4.5f);

            Vector3 leaderPosition = nodes[0].Position;
            nodes[1].Direction = leaderPosition + lVector;
            nodes[2].Direction = leaderPosition + rVector;
        }

        public static void Triangle(List<MAVLinkNode> nodes)
        {
            Console.WriteLine("Triangle");
            float scale = 0.00001f;
            
            Vector3 fVector = Vector3.Normalize(nodes[0].Position);

            float x = (float) (Math.Cos(-90 * Degree2Radian) * fVector.X - Math.Sin(-90 * Degree2Radian) * fVector.Y);
            float y = (float) (Math.Sin(-90 * Degree2Radian) * fVector.X + Math.Cos(-90 * Degree2Radian) * fVector.Y);
            Vector3 rVector = Vector3.Normalize(new Vector3(x, y, 0));

            Vector3 bVector = fVector * -1;
            Vector3 lVector = rVector * -1;

            // 리더의 앞과 오른쪽 벡터 계산
            fVector *= scale;
            bVector *= (scale * 1.5f);
            rVector *= (scale * 2);
            lVector *= (scale * 2);

            Vector3 leaderPosition = nodes[0].Position;
            nodes[1].Direction = leaderPosition + bVector + lVector;
            nodes[2].Direction = leaderPosition + bVector + rVector;
        }

        public void RunScenario()
        {
            MAVLinkNodes.Sort((x, y) => x.IsLeader ? 0 : 1);
            MAVLinkNode leaderNode = MAVLinkNodes[0];

            // Upload mission to leader.
            // leaderNode.UploadMission();

            // Takeoff drones.
            // foreach (MAVLinkNode node in MAVLinkNodes) node.TakeoffCommand();

            leaderNode.ChangeSpeed(1.0f);

            // Start mission.
            leaderNode.StartMission();

            System.Threading.Thread thread = new System.Threading.Thread(() => {

                while (!leaderNode.HasCompletedMission())
                {
                    Flocking();
                    System.Threading.Thread.Sleep(2000);
                }

                foreach (MAVLinkNode node in MAVLinkNodes)
                {
                    node.ClearMission();
                    node.LandCommand();
                }

                leaderNode.ResetMissionReachedSequence();
            });
            thread.Start();
        }
    }
}