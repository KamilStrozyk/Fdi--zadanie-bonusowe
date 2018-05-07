using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;


public class MainParticleScript : MonoBehaviour {
    public Transform[,] pos;
    public Transform[,] ped;
    public InputField Inf;
    public Text mes;
    public Slider s;
    public float time;
    public int n=2;
    public int R;
    public int P;
    public int r;
    public float timer = 0.0f;
    public List<string> States;
    public List<int> count;
    public Text prawddyn;
    public float _prawddyn;
    string meesage;
    public Text entropia;    
    public string _entropia;
    public bool isStart = false;
    public int mnoznik = 0;
    int lnmnoznik;

    public void Init (int _r) { //funkcja rozpoczynajaca, _r to nasza wartość r
        r = _r;
        P = r + 1 - r % 2;
            R = 2 * r + 1;
        n=int.Parse(Inf.text); //n to współczynnik we wzorze na tj
        time = 1 / (float)(n * P);
        string path = @"WykresEntropii-FDI\bin\Debug\Wyniki.txt";
        File.Delete(path); //usuwanie starego pliku wynikowego i generowanie nowego pustego
        File.Create(path);
        isStart = true; //uruchomienie Generowania ruchu
     
    }
    //silnia napisana iteracyjnie
    int silniaI(int x)
    {
        int wynik = 1;

        for(int i=1;i<=x;i++)
        {
            wynik *= i;
            while(wynik>10000)
                //żeby nie wyjść poza zakres pojemnosci zmiennych zastosowane przyblizenie-
                //jesli nasz wynik przekroczy 10k to dzielimy go przez 10 i zwiekszamy zmienna mnoznik,
                //przez co wychodzi nam wynik zaokrąglony i zapisany w notacji wykładniczej
            {

                mnoznik++;
                wynik /= 10;
            }
        }
        return wynik;
    }
    //standardowa funckja silni
        int silnia(int x)
    {
        if (x == 1 || x == 0) return 1;
        else return x * silnia(x - 1);
    }
    float Mianownik() //obliczanie mianownika dla entropii
    {
        float y=1;
        foreach (int el in count)
        {
            y *= silnia(el);
           

        }
        return y;
    }
    //obliczanie ln z "reszty" iloczynu za pomocą funkcji wbudowanych powodowało błędy. 
    //Rozwiazanie: ln(10) w przybliżeniu to  2.30258509299- rozkładam ln(10^x) na ln(10)+ln(10)+...+ln(10) i obliczam
    float ln(int x)
    {
        float wynik = 1;
        while(x>0)
        {

            wynik += 2.30258509299f;
    
                x--;
        }
        return wynik;
    }
    void UpdateStatus()

    { 
      
        meesage = "";
        //wypisanie stanów
        for (int i = 1; i <= States.Count; i++)
        {
            meesage += "n" + i + ":" + States[i - 1] + ";" + count[i - 1] + "el.\n";

        }
        mnoznik = 0;

        //obliczanie prawdopodobieństwa termodynamicznego i wyświetlanie go
      
        _prawddyn=silniaI((int)Mathf.Pow(2, r));
       
        prawddyn.text = "P(tj):" + (silniaI((int)Mathf.Pow(2, r)) / Mianownik()).ToString() +"*10^"+mnoznik.ToString();

        //obliczanie entropii i wysiwetlanie
        //aby obejść ograniczenia wielkości zmiennych, wzór entropii został rozłożony zgodnie z własnościami logarytmu 
        _entropia= (Mathf.Log(_prawddyn) + ln(mnoznik) - Mathf.Log(Mianownik())).ToString();
        entropia.text = "S(tj):" + _entropia;

        //zapisywanie do pliku tekstowego celem generowania wykresu
        string path = @"WykresEntropii-FDI\bin\Debug\Wyniki.txt";
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(_entropia+"\n");
        writer.Close();
        //resetowanie wszystkich zmiennych przed nastepnym stanem
        mes.text = meesage;
        count.Clear();
        States.Clear();
        BroadcastMessage("ChangePos");
        timer = 0;


    }
	// Update is called once per frame
	void Update () {
        
        timer += Time.deltaTime;
        if(timer>=1&&isStart)
        {
            UpdateStatus();
       
        }
        Time.timeScale = s.value;
    }
    public void List(string s)
        //uzupelnianie listy stanów
    {
        if (States.Contains(s)) count[States.IndexOf(s)]++;
        else
        {
            States.Add(s);
            count.Add(1);
        }

    }
    public void Reset() //Resetowanie ukladu
    {
        Application.LoadLevel(Application.loadedLevel);
    }
}
