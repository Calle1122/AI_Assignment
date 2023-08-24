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
        public GameObject targetPointer;
        
        #region Properties

        public new Team_Carl_Lindstedt Team => base.Team as Team_Carl_Lindstedt;

        #endregion

        protected override Unit SelectTarget(List<Unit> enemiesInRange)
        {
            // pick a random target!
            return enemiesInRange != null && enemiesInRange.Count > 0 ? 
                enemiesInRange[Random.Range(0, enemiesInRange.Count)] : null;
        }

        protected override void Start()
        {
            base.Start();

            StartCoroutine(PathfindingLogic());
        }

        protected override void Update()
        {
            base.Update();

            if (targetPointer != null)
            {
                List<Unit> enemyUnits = new List<Unit>(Team.EnemyTeam.Units);
                List<Unit> myUnits = new List<Unit>(Team.Units);

                foreach (var unit in myUnits)
                {
                    enemyUnits.Add(unit);
                }
                targetPointer.transform.position = GetAveragePosition(enemyUnits);
            }
        }

        IEnumerator PathfindingLogic()
        {
            while (true)
            {
                TargetNode = null;
                yield return new WaitForSeconds(Random.Range(1.0f, 2.0f));
                
                List<Unit> enemyUnits = new List<Unit>(Team.EnemyTeam.Units);
                List<Unit> myUnits = new List<Unit>(Team.Units);

                foreach (var unit in myUnits)
                {
                    enemyUnits.Add(unit);
                }
                
                TargetNode = GraphUtils.GetClosestNode<Battlefield.Node>(Battlefield.Instance, GetAveragePosition(enemyUnits));
                yield return new WaitForSeconds(Random.Range(1.0f, 2.0f));
            }
        }
        
        private Vector3 GetAveragePosition(List<Unit> unitsToAverage)
        {
            if(unitsToAverage.Count == 0)
            {
                return Vector3.zero;
            }
 
            Vector3 averageVector = Vector3.zero;
 
            foreach(Unit unit in unitsToAverage)
            {
                averageVector += unit.transform.position;
            }
 
            return (averageVector / unitsToAverage.Count);
        }
    }
}