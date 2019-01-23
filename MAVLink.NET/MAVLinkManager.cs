using System;
using System.Collections.Generic;

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

        private static readonly double World2Local = 100000;
        
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

        public MAVLinkNode RegisterAgent(string port, int baud, byte systemId=1, byte componentId=1)
        {
            MAVLinkNode node = new MAVLinkNode(port, baud, systemId, componentId);
            MAVLinkNodes.Add(node);

            return node;
        }

        public void RegisterAgent(MAVLinkNode node)
        {
            MAVLinkNodes.Add(node);
        }

        public MAVLinkNode FindNodeWithPortName(string portName)
        {
            foreach (MAVLinkNode mavlinkNode in MAVLinkNodes)
                if (mavlinkNode.PortName.Equals(portName))
                    return mavlinkNode;
            return null;
        }

        /****************************************
         * Serial
         ****************************************/
        public void Open(int index=0)
        {
            MAVLinkNode node = MAVLinkNodes[index];

            try
            {
                if (!node.Serial.IsOpen)
                    node.Serial.Open();
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

            node.heartbeatWorker.RunWorkerAsync();
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
                node._is_leader = false;
            MAVLinkNodes[index]._is_leader = true;
        }

        /****************************************
         * Reynold
         ****************************************/
        public static Vector3 Alignment(List<MAVLinkNode> nodes)
        {
            double x = nodes[0].Direction.X;
            double y = nodes[0].Direction.Y;
            return new Vector3(x, y);
        }

        public static Vector3 Separate(Vector3 self, Vector3 other1, Vector3 other2)
        {
            Vector3 vector1 = (self - other1);
            Vector3 vector2 = (self - other2);
            double x = vector1.Size();
            double y = vector2.Size();
            double kValue1 = x / (x + y);
            double kValue2 = y / (x + y);
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
            double kAlignment   = 1;
            double kSeperation  = 1;
            double kCohesion    = 1;

            Vector3 flockCenter = new Vector3();
            for (int i = 0; i < MAVLinkNodes.Count; i++)
                flockCenter += MAVLinkNodes[i].Position;
            flockCenter /= MAVLinkNodes.Count;

            double[,] distance = new double[3, 3];
            for (int i = 0; i < MAVLinkNodes.Count; i++) for (int j = 0; j < MAVLinkNodes.Count; j++)
                {
                    distance[i, j] = (MAVLinkNodes[i].Position * World2Local - MAVLinkNodes[j].Position * World2Local).Size();
                    Console.WriteLine("distance[{0:d}][{1:d}]: {2:f6}", i, j, distance[i, j]);
                }

            Vector3 aVector2 = Alignment(MAVLinkNodes).Normalized();
            Vector3 sVector2 = Separate(MAVLinkNodes[1].Position, MAVLinkNodes[0].Position, MAVLinkNodes[2].Position).Normalized();
            Vector3 cVector2 = Cohesion(MAVLinkNodes[1].Position, flockCenter).Normalized();

            Vector3 aVector3 = Alignment(MAVLinkNodes).Normalized();
            Vector3 sVector3 = Separate(MAVLinkNodes[2].Position, MAVLinkNodes[0].Position, MAVLinkNodes[1].Position).Normalized();
            Vector3 cVector3 = Cohesion(MAVLinkNodes[2].Position, flockCenter).Normalized();

            if (distance[1, 2] < 0.25 || distance[1, 0] < 0.25)
            {
                kAlignment = 0.2;
                kSeperation = 1;
                kCohesion = 0;

                aVector2 *= kAlignment;
                sVector2 *= kSeperation;
                cVector2 *= kCohesion;

                MAVLinkNodes[1].Direction = MAVLinkNodes[1].Position + (aVector2 + sVector2 + cVector2).Normalized() / World2Local;
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
                kAlignment = 0.2;
                kSeperation = 1;
                kCohesion = 0;

                aVector3 *= kAlignment;
                sVector3 *= kSeperation;
                cVector3 *= kCohesion;

                MAVLinkNodes[2].Direction = MAVLinkNodes[2].Position + (aVector3 + sVector3 + cVector3).Normalized() / World2Local;
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
            double scale = 0.00001;
            
            Vector3 fVector = nodes[0].Position.Normalized();
            Vector3 bVector = fVector * -1;

            bVector *= (scale * 2);

            Vector3 leaderPosition = nodes[0].Position;
            nodes[1].Direction = leaderPosition + bVector;
            nodes[2].Direction = leaderPosition + (bVector * 3);
        }

        public static void Column(List<MAVLinkNode> nodes)
        {
            double scale = 0.00001;
            
            Vector3 fVector = nodes[0].Position.Normalized();

            double x = Math.Cos(-90 * Degree2Radian) * fVector.X - Math.Sin(-90 * Degree2Radian) * fVector.Y;
            double y = Math.Sin(-90 * Degree2Radian) * fVector.X + Math.Cos(-90 * Degree2Radian) * fVector.Y;
            Vector3 rVector = new Vector3(x, y).Normalized();

            Vector3 lVector = rVector * -1;
            
            rVector *= (scale * 4.5);
            lVector *= (scale * 4.5);

            Vector3 leaderPosition = nodes[0].Position;
            nodes[1].Direction = leaderPosition + lVector;
            nodes[2].Direction = leaderPosition + rVector;
        }

        public static void Triangle(List<MAVLinkNode> nodes)
        {
            Console.WriteLine("Triangle");
            double scale = 0.00001;
            
            Vector3 fVector = nodes[0].Position.Normalized();

            double x = Math.Cos(-90 * Degree2Radian) * fVector.X - Math.Sin(-90 * Degree2Radian) * fVector.Y;
            double y = Math.Sin(-90 * Degree2Radian) * fVector.X + Math.Cos(-90 * Degree2Radian) * fVector.Y;
            Vector3 rVector = new Vector3(x, y).Normalized();

            Vector3 bVector = fVector * -1;
            Vector3 lVector = rVector * -1;

            // 리더의 앞과 오른쪽 벡터 계산
            fVector *= scale;
            bVector *= (scale * 1.5);
            rVector *= (scale * 2);
            lVector *= (scale * 2);

            Vector3 leaderPosition = nodes[0].Position;
            nodes[1].Direction = leaderPosition + bVector + lVector;
            nodes[2].Direction = leaderPosition + bVector + rVector;
        }

        public void RunScenario()
        {
            MAVLinkNodes.Sort((x, y) => x._is_leader ? 0 : 1);
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