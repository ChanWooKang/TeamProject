using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechnologyManager : TSingleton<TechnologyManager>
{
    int m_technologyLevel;


    public void TechLevelUp()
    {
        ++m_technologyLevel;        
    }
}
