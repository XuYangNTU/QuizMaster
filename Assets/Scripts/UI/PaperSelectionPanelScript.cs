using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public class PaperSelectionPanelScript : MonoBehaviour
{
    [SerializeField] private Toggle m_togglePref;

    [SerializeField] private ToggleGroup m_toggleGroup;
    [SerializeField] private Transform m_togglesHolder;


    public string SelectedFile
    {
        get { return m_toggleGroup.ActiveToggles().First().GetComponentInChildren<Text>().text + ".tsv"; }
    }



    public void PopulatePanel(string folderPath)
    {
        //clear the panel
        int child_count = m_togglesHolder.childCount;
        for (int i = 0; i < child_count; i++)
        {
            Destroy(m_togglesHolder.GetChild(i).gameObject);
        }

        //get files
        string[] file_paths = Directory.GetFiles(folderPath);
        foreach (string file_path in file_paths)
        {
            string name = Path.GetFileNameWithoutExtension(file_path);
            Toggle toggle = Instantiate(m_togglePref, m_togglesHolder);
            toggle.GetComponentInChildren<Text>().text = name;
            toggle.group = m_toggleGroup;
            toggle.transform.SetAsFirstSibling();
        }   
    }

}
