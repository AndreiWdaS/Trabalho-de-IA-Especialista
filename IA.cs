using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IA : MonoBehaviour
{
    public Text texto;
    public TMP_InputField resposta;
    public database data;
    public typewriterUI escritor;
    private int Ordem;
    private int menorAno = 9999;
    private int maiorAno;
    private int menorDura = 9999;
    private int maiorDura;
    public Dictionary<string, float> Notas = new Dictionary<string, float>();
    public List<float> Multiplicadores = new List<float>();

    // Start is called before the first frame update
    void Start()
    {
        if (resposta != null)
        {
            resposta.onSubmit.AddListener(OnInputFieldSubmitted);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Ordem == 0)
        {
            foreach (List<string> filme in data.filmes)
            {
                if(int.Parse(filme[1]) < menorAno)
                {
                    menorAno = int.Parse(filme[1]);
                }
                if(int.Parse(filme[1]) > maiorAno)
                {
                    maiorAno = int.Parse(filme[1]);
                }
            }
            escritor.Comecar( 
                "                Ola! Sou uma inteligencia artificial especialista, meu proposito eh lhe ajudar a encontrar um filme de " +
                "horror/terror do seu gosto! Vamos comecar? Primeiro de tudo, voce quer um filme de mais ou menos qual ano? Por favor, " +
                "digite algum ano entre " +
                menorAno + " e " + maiorAno + "."
            );
            Ordem = 1;
        }
        if(Input.GetKeyDown("escape")){
            Application.Quit();
        }
        if(escritor.rotinaUm == true || escritor.rotinaDois == true){
            resposta.interactable = false;
        }
        else{
            resposta.interactable = true;
        }
    }
    private void OnInputFieldSubmitted(string text)
    {
        bool firstOne = true;
        switch(Ordem){
            case 1:
                firstOne = true;
                foreach (List<string> filme in data.filmes)
                {
                    if(firstOne == false)
                    {
                        float etapa = Math.Abs(int.Parse(filme[1]) - int.Parse(resposta.text));
                        if(Math.Abs(int.Parse(resposta.text) - menorAno) < Math.Abs(int.Parse(resposta.text) - maiorAno)){
                            etapa = etapa / Math.Abs(int.Parse(resposta.text) - maiorAno);
                        }
                        else{
                            etapa = etapa / Math.Abs(int.Parse(resposta.text) - menorAno);
                        }
                        Multiplicadores.Add(1 - etapa);
                    }
                    firstOne = false;
                }
                foreach (List<string> filme in data.filmes)
                {
                    if(int.Parse(filme[2]) < menorDura)
                    {
                        menorDura = int.Parse(filme[2]);
                    }
                    if(int.Parse(filme[2]) > maiorDura)
                    {
                        maiorDura = int.Parse(filme[2]);
                    }
                }
                int menorHoras = menorDura / 60; // Divide para obter as horas
                int maiorHoras = maiorDura / 60; // Divide para obter as horas
                int menorRestantes = menorDura % 60; // Obtem o resto da divisão para os minutos restantes
                int maiorRestantes = maiorDura % 60; // Obtem o resto da divisão para os minutos restantes

                string menorFormatada = string.Format("{0:00}:{1:00}", menorHoras, menorRestantes);
                string maiorFormatada = string.Format("{0:00}:{1:00}", maiorHoras, maiorRestantes);
                escritor.Comecar(
                    "\n\n                O quao longo o filme deve ser em media? Coloque um valor entre " + menorFormatada + " e " +
                    maiorFormatada + ". Por favor, escreva a duracao em minutos, por exemplo, um filme de duas horas como 120."
                );
                Ordem ++;
            break;
            case 2:
                firstOne = true;
                foreach (List<string> filme in data.filmes)
                {
                    if(firstOne == false)
                    {
                        float etapa = Math.Abs(int.Parse(filme[2]) - int.Parse(resposta.text));
                        if(Math.Abs(int.Parse(resposta.text) - menorDura) < Math.Abs(int.Parse(resposta.text) - maiorDura)){
                            etapa = etapa / Math.Abs(int.Parse(resposta.text) - maiorDura);
                        }
                        else{
                            etapa = etapa / Math.Abs(int.Parse(resposta.text) - menorDura);
                        }
                        Multiplicadores[data.filmes.IndexOf(filme) - 1] += 1 - etapa;
                    }
                    firstOne = false;
                }
                escritor.Comecar( 
                    "\n\n                Esta buscando por diretores expecificos? Se sim, escreva seus nomes, separando os mesmos com uma virgula sem espaco depois!"
                );
                Ordem ++;
            break;
            case 3:
                firstOne = true;
                foreach (List<string> filme in data.filmes)
                {
                    if(firstOne == false)
                    {
                        string[] nome = resposta.text.Split(",");
                        
                        if(nome.Contains(filme[5])){
                            Multiplicadores[data.filmes.IndexOf(filme) - 1] += 10;
                        }
                    }
                    firstOne = false;
                }
                escritor.Comecar( 
                    "\n\n                Muito bem, agora irei fazer uma serie de perguntas, por favor, responda todas elas com um numero de " +
                    "0 a 10. Leve em concideracao que 5 eh a mesma coisa que 'nao importa', 0 a mesma coisa que 'inaceitavel' e 10 a mesma " +
                    "coisa que 'obrigatorio'!\n\n                Vamos comecar, o quao realista voce quer que o filme seja?"
                );
                Ordem ++;
            break;
            case 4:
                firstOne = true;
                foreach (List<string> filme in data.filmes)
                {
                    if(firstOne == false)
                    {
                        if(data.generos[filme[0]].Contains("Biography") || data.generos[filme[0]].Contains("History")){
                            Notas.Add(filme[0], int.Parse(resposta.text) - 5 + 1);
                        }
                        if(data.generos[filme[0]].Contains("Fantasy") || data.generos[filme[0]].Contains("Sci-Fi") ||
                        data.generos[filme[0]].Contains("Musical") || data.generos[filme[0]].Contains("Music") ||
                        data.generos[filme[0]].Contains("Animation")){
                            Notas.Add(filme[0], -int.Parse(resposta.text) + 5 + 1);
                        }
                        else if(!data.generos[filme[0]].Contains("Biography") && !data.generos[filme[0]].Contains("History")){
                            Notas.Add(filme[0], 1);
                        }
                    }
                    firstOne = false;
                }
                escritor.Comecar( 
                    "\n\n                Quer comedia no filme?"
                );
                Ordem ++;
            break;
            case 5:
                firstOne = true;
                foreach (List<string> filme in data.filmes)
                {
                    if(firstOne == false)
                    {
                        if(data.generos[filme[0]].Contains("Comedy")){
                            Notas[filme[0]] += int.Parse(resposta.text) - 5;
                        }
                        if(data.generos[filme[0]].Contains("Horror") || data.generos[filme[0]].Contains("Thriller") ||
                        data.generos[filme[0]].Contains("Drama") || data.generos[filme[0]].Contains("Mistery") ||
                        data.generos[filme[0]].Contains("War")){
                            Notas[filme[0]] += -int.Parse(resposta.text) + 5;
                        }
                    }
                    firstOne = false;
                }
                escritor.Comecar( 
                    "\n\n                Quer acao no filme?"
                );
                Ordem ++;
            break;
            case 6:
                firstOne = true;
                foreach (List<string> filme in data.filmes)
                {
                    if(firstOne == false)
                    {
                        if(data.generos[filme[0]].Contains("Action") || data.generos[filme[0]].Contains("Adventure") ||
                        data.generos[filme[0]].Contains("Western")){
                            Notas[filme[0]] += int.Parse(resposta.text) - 5;
                        }
                        if(data.generos[filme[0]].Contains("Mistery") || data.generos[filme[0]].Contains("Drama") ||
                        data.generos[filme[0]].Contains("Mistery")){
                            Notas[filme[0]] += -int.Parse(resposta.text) + 5;
                        }
                    }
                    firstOne = false;
                }
                escritor.Comecar( 
                    "\n\n                Quer romance no filme?"
                );
                Ordem ++;
            break;
            case 7:
            firstOne = true;
                foreach (List<string> filme in data.filmes)
                {
                    if(firstOne == false)
                    {
                        if(data.generos[filme[0]].Contains("Romance") || data.generos[filme[0]].Contains("Drama")){
                            Notas[filme[0]] += int.Parse(resposta.text) - 5;
                        }
                    }
                    firstOne = false;
                }
                escritor.Comecar( 
                    "\n\n                O quao tenso voce quer que o filme seja?"
                );
                Ordem ++;
            break;
            case 8:
            escritor.Comecar("\n\n                Muito bem, segue a lista dos 10 filmes mais compativeis com os dados fornecidos:\n\n");
            int i = 0;
            foreach(string filmeNota in Notas.Keys.ToList())
            {
                Notas[filmeNota] = Notas[filmeNota] * Multiplicadores[i];
                i++;
            };
            StartCoroutine(final(0));

            Ordem ++;
            break;
        }
    }
    public IEnumerator final(int i)
    {
        List<KeyValuePair<string, float>> listaNotas = Notas.ToList();
        listaNotas = listaNotas.OrderByDescending(pair => pair.Value).Take(10).ToList();
        escritor.rotinaUm = true;
        escritor.rotinaUm = true;
        while(i < 10)
        {
            if(escritor.rotinaUm == false && escritor.rotinaDois == false)
            {
                escritor.rotinaUm = true;
                escritor.rotinaUm = true;
                float valorEscolhido = MathF.Round(((listaNotas[i].Value / 40) * 100), 2);
                escritor.Comecar(valorEscolhido + "% - " + listaNotas[i].Key + "\n");
                yield return new WaitForEndOfFrame();
                i ++;
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
