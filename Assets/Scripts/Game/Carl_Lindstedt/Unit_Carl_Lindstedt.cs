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
                        
                        Debug.Log("Updated new enemy target");
                    }
                }
            }

            return targetEnemy;
        }

        protected override GraphUtils.Path GetPathToTarget()
        {
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
                    TargetNode = GraphUtils.GetClosestNode<Battlefield.Node>(Battlefield.Instance,
                        Team.squadLeaderTargetPoint.transform.position);
                    yield return new WaitForSeconds(.1f);
                }

                else
                {
                    TargetNode = GraphUtils.GetClosestNode<Battlefield.Node>(Battlefield.Instance,
                        Team.targetPoints[FormationNumber].transform.position);
                    yield return new WaitForSeconds(.1f);
                }
            }
        }
    }
}