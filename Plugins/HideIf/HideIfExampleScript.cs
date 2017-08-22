using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideIfExampleScript : MonoBehaviour {
    
    public bool showS1;
    
    [HideIf("showS1", false)]
    public string s1 = "S1";
    [HideIf("showS1", true)]
    public string s2 = "S2 instead";
    
    [Space(10f)]
    public Object obj;
    
    [HideIfNotNull("obj")]
    public string objIsNull = "Shown as obj is null!";
    
    [HideIfNull("obj")]
    public string objIsNotNull = "Shown as obj is not null!";
    
    [Space(10f)]
    public TestEnum testEnum;
    
    [HideIfEnumValue("testEnum", HideIf.Equal, (int) TestEnum.Val1)]
    public string isNotVal1 = "Val 1 not selected";
    
    [HideIfEnumValue("testEnum", HideIf.NotEqual, (int) TestEnum.Val1)]
    public string isVal1 = "Val 1 selected";
    
    [HideIfEnumValue("testEnum", HideIf.Equal, (int) TestEnum.Val2, (int) TestEnum.Val3)]
    public string isNotVal2Or3 = "Neither Val 2 nor 3 are selected";
    
    [HideIfEnumValue("testEnum", HideIf.NotEqual, (int) TestEnum.Val2, (int) TestEnum.Val3)]
    public string isVal2Or3 = "Val 2 or 3 are selected";
    
    public enum TestEnum {
        Val1,
        Val2,
        Val3
    }
}
