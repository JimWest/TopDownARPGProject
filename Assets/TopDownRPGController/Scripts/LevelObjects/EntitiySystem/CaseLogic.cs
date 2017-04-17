using UnityEngine;
#if UNITY_EDITOR 
using UnityEditor;
#endif

namespace TopDown.EntitySystem
{
    // cases as a struct for better a better inspector (will be collapseable)
    [System.Serializable]
    public struct Cases
    {
        public string case01;
        public string case02;
        public string case03;
        public string case04;
        public string case05;
        public string case06;
        public string case07;
        public string case08;
        public string case09;
        public string case10;
        public string case11;
        public string case12;
        public string case13;
        public string case14;
        public string case15;
        public string case16;
    }

    public class CaseLogic : EntityEventTrigger
    {
        [SerializeField]
        Cases _cases;

#if UNITY_EDITOR
        [MenuItem("GameObject/Entity/CaseLogic", false, 10)]
        static void CreateCustomGameObject(MenuCommand menuCommand)
        {
            CreateGameObject<CaseLogic>(menuCommand);
        }
#endif

        public void InValue(string value)
        {
            if (string.Compare(value, _cases.case01) == 0)
                OnCase01();
            if (string.Compare(value, _cases.case02) == 0)
                OnCase02();
            if (string.Compare(value, _cases.case03) == 0)
                OnCase03();
            if (string.Compare(value, _cases.case04) == 0)
                OnCase04();
            if (string.Compare(value, _cases.case05) == 0)
                OnCase05();
            if (string.Compare(value, _cases.case06) == 0)
                OnCase06();
            if (string.Compare(value, _cases.case07) == 0)
                OnCase07();
            if (string.Compare(value, _cases.case08) == 0)
                OnCase08();
            if (string.Compare(value, _cases.case09) == 0)
                OnCase09();
            if (string.Compare(value, _cases.case10) == 0)
                OnCase10();
            if (string.Compare(value, _cases.case11) == 0)
                OnCase11();
            if (string.Compare(value, _cases.case12) == 0)
                OnCase12();
            if (string.Compare(value, _cases.case13) == 0)
                OnCase13();
            if (string.Compare(value, _cases.case14) == 0)
                OnCase14();
            if (string.Compare(value, _cases.case15) == 0)
                OnCase15();
            if (string.Compare(value, _cases.case16) == 0)
                OnCase16();
        }

        public void PickRandom()
        {
            // Don't do anything when there are no otputs
            if (Triggers.Count == 0)
                return;

            // get a random output
            int randomIndex = UnityEngine.Random.Range(1, Triggers.Count + 1) - 1;

            // find the eventID and call the output
            if (Triggers[randomIndex] != null)
            {

                switch (Triggers[randomIndex].eventID)
                {
                    case 0:
                        OnCase01();
                        break;
                    case 1:
                        OnCase02();
                        break;
                    case 2:
                        OnCase03();
                        break;
                    case 3:
                        OnCase04();
                        break;
                    case 4:
                        OnCase05();
                        break;
                    case 5:
                        OnCase06();
                        break;
                    case 6:
                        OnCase07();
                        break;
                    case 7:
                        OnCase08();
                        break;
                    case 8:
                        OnCase09();
                        break;
                    case 9:
                        OnCase10();
                        break;
                    case 10:
                        OnCase11();
                        break;
                    case 11:
                        OnCase12();
                        break;
                    case 12:
                        OnCase13();
                        break;
                    case 13:
                        OnCase14();
                        break;
                    case 14:
                        OnCase15();
                        break;
                    case 15:
                        OnCase16();
                        break;
                }
            }

        }

        [EntityOutputEvent]
        void OnCase01()
        {
            Execute("OnCase01");
        }

        [EntityOutputEvent]
        void OnCase02()
        {
            Execute("OnCase02");
        }

        [EntityOutputEvent]
        void OnCase03()
        {
            Execute("OnCase03");
        }

        [EntityOutputEvent]
        void OnCase04()
        {
            Execute("OnCase04");
        }

        [EntityOutputEvent]
        void OnCase05()
        {
            Execute("OnCase05");
        }

        [EntityOutputEvent]
        void OnCase06()
        {
            Execute("OnCase06");
        }

        [EntityOutputEvent]
        void OnCase07()
        {
            Execute("OnCase07");
        }

        [EntityOutputEvent]
        void OnCase08()
        {
            Execute("OnCase08");
        }

        [EntityOutputEvent]
        void OnCase09()
        {
            Execute("OnCase09");
        }

        [EntityOutputEvent]
        void OnCase10()
        {
            Execute("OnCase10");
        }

        [EntityOutputEvent]
        void OnCase11()
        {
            Execute("OnCase11");
        }

        [EntityOutputEvent]
        void OnCase12()
        {
            Execute("OnCase12");
        }

        [EntityOutputEvent]
        void OnCase13()
        {
            Execute("OnCase13");
        }

        [EntityOutputEvent]
        void OnCase14()
        {
            Execute("OnCase14");
        }

        [EntityOutputEvent]
        void OnCase15()
        {
            Execute("OnCase15");
        }

        [EntityOutputEvent]
        void OnCase16()
        {
            Execute("OnCase16");
        }
    }
}
