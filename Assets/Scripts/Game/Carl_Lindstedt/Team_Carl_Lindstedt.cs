using System;
using Game;
using Graphs;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Carl_Lindstedt
{
    public class Team_Carl_Lindstedt : Team
    {
        [SerializeField]
        private Color   m_myFancyColor;

        public GameObject squadLeaderTargetPoint;
        
        public List<GameObject> targetPoints;
        
        public bool formationFollow = true;
        
        private List<Unit> friendlyUnits;
        private int lastFrameAliveFriendlies;
        
        #region Properties

        public override Color Color => m_myFancyColor;

        #endregion

        protected override void Start()
        {
            base.Start();

            //Gets all friendly units
            friendlyUnits = GetFriendlyTeamList();
            
            //Set the units formation numbers to their new positions
            for (int i = 0; i < friendlyUnits.Count; i++)
            {
                Unit_Carl_Lindstedt myUnit = friendlyUnits[i] as Unit_Carl_Lindstedt;
                myUnit.FormationNumber = i;
            }
        }

        private void Update()
        {
            if (Units.Count() != lastFrameAliveFriendlies)
            {
                friendlyUnits = GetFriendlyTeamList();
                ShuffleFormation();
            }
            else
            {
                //Have the main formation point follow the squad leader (if bool true)
                if (formationFollow)
                {
                    targetPoints[0].transform.position = friendlyUnits[0].transform.position;
                }
            }
            
            //Get alive enemies & friendlies
            List<Unit> enemyUnits = GetEnemyTeamList();
            
            //Rotate formation points towards the enemy average position
            Vector3 enemyAveragePosition = GetAveragePosition(enemyUnits);
            targetPoints[0].transform.LookAt(enemyAveragePosition, transform.up);
            
            //Update the squad leaders target position
            List<Unit> allAliveUnits = new List<Unit>();
            allAliveUnits.AddRange(friendlyUnits);
            allAliveUnits.AddRange(enemyUnits);
            squadLeaderTargetPoint.transform.position = GetAveragePosition(allAliveUnits);
            
            //Set last known number of alive friendlies
            lastFrameAliveFriendlies = friendlyUnits.Count;
        }

        public List<Unit> GetFriendlyTeamList()
        {
            List<Unit> newFriendlyUnits = new List<Unit>(Units);
            return newFriendlyUnits;
        }
        
        public List<Unit> GetEnemyTeamList()
        {
            List<Unit> enemyUnits = new List<Unit>(EnemyTeam.Units);
            return enemyUnits;
        }
        
        public void ShuffleFormation()
        {
            StartCoroutine(FreezeFormation());
            
            List<Unit> newUnitFormation = new List<Unit>();
            //Shuffle the list
            switch (friendlyUnits.Count)
            {
                case 5:
                    newUnitFormation.Add(friendlyUnits[1]);
                    newUnitFormation.Add(friendlyUnits[2]);
                    newUnitFormation.Add(friendlyUnits[3]);
                    newUnitFormation.Add(friendlyUnits[4]);
                    newUnitFormation.Add(friendlyUnits[0]);
                    break;
                
                case 4:
                    newUnitFormation.Add(friendlyUnits[1]);
                    newUnitFormation.Add(friendlyUnits[2]);
                    newUnitFormation.Add(friendlyUnits[3]);
                    newUnitFormation.Add(friendlyUnits[0]);
                    break;
                
                case 3:
                    newUnitFormation.Add(friendlyUnits[1]);
                    newUnitFormation.Add(friendlyUnits[2]);
                    newUnitFormation.Add(friendlyUnits[0]);
                    break;
                
                case 2:
                    newUnitFormation.Add(friendlyUnits[1]);
                    newUnitFormation.Add(friendlyUnits[0]);
                    break;
                
                case 1:
                    newUnitFormation.Add(friendlyUnits[0]);
                    break;
            }

            friendlyUnits = newUnitFormation;

            //Set the units formation numbers to their new positions
            for (int i = 0; i < friendlyUnits.Count; i++)
            {
                Unit_Carl_Lindstedt myUnit = friendlyUnits[i] as Unit_Carl_Lindstedt;
                myUnit.FormationNumber = i;
            }
        }
        
        IEnumerator FreezeFormation()
        {
            formationFollow = false;
            yield return new WaitForSeconds(5f);
            formationFollow = true;
        }

        public Vector3 GetAveragePosition(List<Unit> unitsToAverage)
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
        
        public GraphUtils.Path GetShortestPath(Battlefield.Node start, Battlefield.Node goal)
        {
            if (start == null ||
                goal == null ||
                start == goal ||
                Battlefield.Instance == null)
            {
                return null;
            }

            // initialize pathfinding
            foreach (Battlefield.Node node in Battlefield.Instance.Nodes)
            {
                node?.ResetPathfinding();
            }

            // add start node
            start.m_fDistance = 0.0f;
            start.m_fRemainingDistance = Battlefield.Instance.Heuristic(goal, start);
            List<Battlefield.Node> open = new List<Battlefield.Node>();
            HashSet<Battlefield.Node> closed = new HashSet<Battlefield.Node>();
            open.Add(start);

            // search
            while (open.Count > 0)
            {
                // get next node (the one with the least remaining distance)
                Battlefield.Node current = open[0];
                for (int i = 1; i < open.Count; ++i)
                {
                    if (open[i].m_fRemainingDistance < current.m_fRemainingDistance)
                    {
                        current = open[i];
                    }
                }
                open.Remove(current);
                closed.Add(current);

                // found goal?
                if (current == goal)
                {
                    // construct path
                    GraphUtils.Path path = new GraphUtils.Path();
                    while (current != null)
                    {
                        path.Add(current.m_parentLink);
                        current = current != null && current.m_parentLink != null ? current.m_parentLink.Source : null;
                    }

                    path.RemoveAll(l => l == null);     // HACK: check if path contains null links
                    path.Reverse();
                    return path;
                }
                else
                {
                    foreach (Battlefield.Link link in current.Links)
                    {
                        if (link.Target is Battlefield.Node target)
                        {
                            if (!closed.Contains(target) &&
                                target.Unit == null)
                            {
                                float newDistance = current.m_fDistance + Vector3.Distance(current.WorldPosition, target.WorldPosition);
                                float newRemainingDistance = newDistance + Battlefield.Instance.Heuristic(target, start);

                                if (open.Contains(target))
                                {
                                    if (newRemainingDistance < target.m_fRemainingDistance)
                                    {
                                        // re-parent neighbor node
                                        target.m_fRemainingDistance = newRemainingDistance;
                                        target.m_fDistance = newDistance;
                                        target.m_parentLink = link;
                                    }
                                }
                                else
                                {
                                    // add target to openlist
                                    target.m_fRemainingDistance = newRemainingDistance;
                                    target.m_fDistance = newDistance;
                                    target.m_parentLink = link;
                                    open.Add(target);
                                }
                            }
                        }
                    }
                }
            }

            // no path found :(
            return null;
        }
    }
}