using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Skrypt kontrolujący pojedyncza czastke
public class ParticleScript : MonoBehaviour
{
    public GameObject Pos; //graficzne przedstawienie na siatce pozycji
    public GameObject Ped;//graficzne przedstawienie na siatce pedu
  public   float[] posF = new float[2]; //pozycja w formacie [x,y] dla przestrzeni
    public int[] pos = new int[2];  //zaokrąglona pozycja w formacie [x,y] dla przestrzeni
    public int[] ped = new int[2];//pozycja w formacie [x,y] dla pedu
    public string state;
    MainParticleScript MPS;

    void Start()
    {
        //Tworzenie dołączenia do MainParticleScript (w skrócie: MPS)
        MPS = GetComponentInParent<MainParticleScript>();
        //pobranie pozycji poczatkowej
        //posF[0] = pos[0];
        //posF[1] = pos[1];

    }
    //Główna funkja- powoduje zmiane pozycji
    void ChangePos()
    {
        //(p=m*V => V= p/m) ^ m=1 ) => V=p
        //generowanie nowej pozycji
        float x = (int)ped[0] * MPS.time;
        float y = (int)ped[1] * MPS.time;
        posF[0] += x;
        posF[1] += y;

        //zaokraglanie- zakłożenie: cząstka znajduje się w danej komórce, jeśli jej współrzędna x (analogicznie y) ma wspołrzedne (X-1;x>
        pos[0] = Mathf.CeilToInt(posF[0]);
        pos[1] = Mathf.CeilToInt(posF[1]);

        //jeśli doleci do ścianki- zatrzymuje się na najbardziej krańcowej komórce, a pęd ulega zmianie zgodnie z treścią zadania
        if (pos[0] > MPS.r)
        {
            pos[0] = MPS.r;
            ped[0] = ped[0] * (-1);
        }
        if (pos[1] > MPS.r)
        {
            pos[1] = MPS.r;
            ped[1] = ped[1] * (-1);
        }
        if (pos[0] < (-1) * MPS.r)
        {
            pos[0] = MPS.r * (-1);
            ped[0] = ped[0] * (-1);
        }
        if (pos[1] < (-1) * MPS.r)
        {
            pos[1] = MPS.r * (-1);
            ped[1] = ped[1] * (-1);
        }

        //Graficzna prezentacja nowej pozycji i pędu
        Transform Nf = MPS.pos[(MPS.r + pos[0]), MPS.r + pos[1]];
        Vector3 NewPos = new Vector3(Nf.position.x, Nf.position.y, Nf.position.z - 1);
        Pos.transform.position = NewPos;
        Transform Np = MPS.ped[(MPS.r / 2 + ped[0]), MPS.r / 2 + ped[1]];
        Vector3 NewPed = new Vector3(Np.position.x, Np.position.y, Np.position.z - 1);
        Ped.transform.position = NewPed;
        //generowanie wiadomosci wyswietlanej w liście stanów i wysyłanie jej do MPS
        state = "(" + pos[0] + "," + pos[1] + "," + ped[0] + "," + ped[1] + ")";
        MPS.SendMessage("List", state);
    }
}
