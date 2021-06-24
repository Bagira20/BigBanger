using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEditor;


namespace TheBigBanger.Formulae
{
    public enum InputFactor
    {
        F, M, A, V,
    }

    public class FormulaSheets
    {
        public static string[] ForceIs =
        {
            "ma",
            "1/2m(v²)",
        };

        public const string tooltip = "Options are: \n1.) ma\n2.) 1/2m(v²)";
    }
}
