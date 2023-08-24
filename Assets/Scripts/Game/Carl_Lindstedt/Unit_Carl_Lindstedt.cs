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
        #region Properties

        public new Team_Carl_Lindstedt Team => base.Team as Team_Carl_Lindstedt;

        #endregion

        protected override Unit SelectTarget(List<Unit> enemiesInRange)
        {
            return enemiesInRange[Random.Range(0, enemiesInRange.Count)];
        }
    }
}