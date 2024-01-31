using System;
using UnityEngine;

//Git
namespace Utilities.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property |
                 AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
    public class ReadOnly : PropertyAttribute
    {
    }
}