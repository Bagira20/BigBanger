using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEditor;


namespace TheBigBanger.Formulae
{
    public enum FactorElement
    {
        F, M, A, V,
    }

    public class FormulaSheets
    {
        public static string[] ForceIs =
        {
            "1/2m(v²)",
            "ma"
        };

        public const string tooltip = "Options are: \n0.) 1/2m(v²)\n1.) ma";
    }
}
