using AI;
using Game;
using Graphs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Carl_Lindstedt
{
    public class Unit_Carl_Lindstedt : Unit
    {
        private int m_formationNumber;

        #region Properties

        public new Team_Carl_Lindstedt Team => base.Team as Team_Carl_Lindstedt;

        public int FormationNumber
        {
            get => m_formationNumber;

            set { m_formationNumber = value; }
        }

        #endregion

        //Returns the unit with the lowest health (of enemies in range)
        protected override Unit SelectTarget(List<Unit> enemiesInRange)
        {
            Unit targetEnemy = enemiesInRange[0];
            float currentMinHealth = MAX_HP;

            if (enemiesInRange.Count > 1)
            {
                foreach (var enemy in enemiesInRange)
                {
                    if (enemy.Health < currentMinHealth)
                    {
                        targetEnemy = enemy;
                        currentMinHealth = enemy.Health;
                    }
                }
            }

            return targetEnemy;
        }

        protected override GraphUtils.Path GetPathToTarget()
        {
            //Returns a shortest path using my overriden function in the Team class
            return Team.GetShortestPath(CurrentNode, TargetNode);
        }

        protected override void Start()
        {
            base.Start();

            StartCoroutine(PathfindingLogic());
        }

        IEnumerator PathfindingLogic()
        {
            while (true)
            {
                //if you are the squad leader... pathfind towards target
                if (FormationNumber == 0)
                {
                    //the "squad leader target point" gets updated from the Team class
                    TargetNode = GraphUtils.GetClosestNode<Battlefield.Node>(Battlefield.Instance,
                        Team.squadLeaderTargetPoint.transform.position);
                    yield return new WaitForSeconds(.1f);
                }

                //if you are not the squad leader... pathfind towards your formation point
                else
                {
                    //the formation point is based on what number in the formation the unit is, as well as where the squad leader is
                    TargetNode = GraphUtils.GetClosestNode<Battlefield.Node>(Battlefield.Instance,
                        Team.targetPoints[FormationNumber].transform.position);
                    yield return new WaitForSeconds(.1f);
                }
            }
        }
    }
}