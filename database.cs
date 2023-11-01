using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class database : MonoBehaviour
{
    public List<List<string>> filmes = new List<List<string>>();
    public Dictionary<string, string[]> generos = new Dictionary<string, string[]>(); // chave = nome do filme
    public List<string> generosDebug = new List<string>(); // chave = nome do filme
    public TextAsset csvFile;
    // Start is called before the first frame update
    void Start()
    {
        string[] csvLines = csvFile.text.Split('\n');
            
        for (int i = 1; i < csvLines.Length; i++)
        {
            string[] values = csvLines[i].Split(',');
            if(values.Length > 3){
                List<string> Temp = new List<string>();
                foreach(string valor in values)
                {
                    string novoValor = valor.Replace("\"", "");
                    while(valor == values[0] && generos.ContainsKey(novoValor)){
                        novoValor = novoValor + "+";
                    }
                    if(valor != values[3]){
                        Temp.Add(novoValor);
                    }
                    else{
                        string[] gens = novoValor.Split(";");
                        foreach(string genero in gens){
                            if(!generosDebug.Contains(genero)){
                                generosDebug.Add(genero);
                            }
                        }
                        generos.Add(Temp[0], gens);
                    }
                }
                filmes.Add(Temp);
            }
        }
    }
}
