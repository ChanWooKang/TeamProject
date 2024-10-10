using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechnologyManager : TSingleton<TechnologyManager>
{
    const int m_levelUPPointOffset = 5;
    int m_technologyLevel;
    int m_technologyPoint;
    [SerializeField] List<GameObject> m_listCraftObjPrefabs;

    public int TechLevel { get { return m_technologyLevel; } }
    public int TechPoint { get { return m_technologyPoint; } }
    public int LevelUpPoint { get { return m_levelUPPointOffset; } }
    public List<GameObject> CraftObjPrefabs { get { return m_listCraftObjPrefabs; } }

    private void Awake()
    {
        m_technologyLevel = 100;
    }
    public void TechLevelUp()
    {
        ++m_technologyLevel;
        m_technologyPoint -= m_levelUPPointOffset;
    }
    public void TechPointUp()
    {
        ++m_technologyPoint;        
    }
}
