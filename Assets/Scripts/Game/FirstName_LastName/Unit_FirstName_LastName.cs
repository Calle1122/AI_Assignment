using AI;
using Game;
using Graphs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FirstName_LastName
{
    public class Unit_FirstName_LastName : Unit
    {
        #region Properties

        public new Team_FirstName_LastName Team => base.Team as Team_FirstName_LastName;

        #endregion

        protected override Unit SelectTarget(List<Unit> enemiesInRange)
        {
            return enemiesInRange[Random.Range(0, enemiesInRange.Count)];
        }
    }
}