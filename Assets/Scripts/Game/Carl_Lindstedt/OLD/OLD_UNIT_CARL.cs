using AI;
using Game;
using Graphs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Carl_Lindstedt
{
    public class OLD_UNIT_CARL : Unit
    {
        private int m_formationNumber;
        
        #region Properties

        public new OLD_TEAM_CARL Team => base.Team as OLD_TEAM_CARL;

        public int FormationNumber
        {
            get => m_formationNumber;

            set
            {
                m_formationNumber = value;
            }
        }
        
        #endregion

        protected override Unit SelectTarget(List<Unit> enemiesInRange)
        {
            // pick a random target!
            return enemiesInRange != null && enemiesInRange.Count > 0 ? 
                enemiesInRange[Random.Range(0, enemiesInRange.Count)] : null;
        }

        protected override GraphUtils.Path GetPathToTarget()
        {
            return Team.OldGetShortestPath(CurrentNode, TargetNode);
        }

        protected override void Start()
        {
            base.Start();

            StartCoroutine(OldPathfindingLogic());
        }

        IEnumerator OldPathfindingLogic()
        {
            while (true)
            {
                //if you are the squad leader... pathfind towards target
                if (FormationNumber == 0)
                {
                    TargetNode = GraphUtils.GetClosestNode<Battlefield.Node>(Battlefield.Instance, Team.squadLeaderTargetPoint.transform.position);
                    yield return new WaitForSeconds(.1f);
                }

                else
                {
                    TargetNode = GraphUtils.GetClosestNode<Battlefield.Node>(Battlefield.Instance, Team.targetPoints[FormationNumber].transform.position);
                    yield return new WaitForSeconds(.1f);
                }
            }
        }
    }
}