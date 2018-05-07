using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Skrypt generujacy wszystkie czastki
public class MakeParticles : MonoBehaviour {
    public InputField inf;
        public int N;
    public int r;
    public Transform Part;
    public Sprite p;
    //Do okreslania pozycji generowania- pobiera wartosci z Grid
    public List<GameObject> Fc = new List<GameObject>();
    
  public  void Init () { //funkcja głowna

        //liczba czastek
        r = int.Parse(inf.text);
        N =(int) System.Math.Pow(2, r);
        //generowanie czastek
        GameObject projectile= new GameObject();
        int count = 1; //zmienna potrzebna do równomiernego rozmieszczenia
        for(int i=1;i<=N;i++)
        {
          


            GameObject p0 = Instantiate(projectile, new Vector2(0f,0f), Quaternion.identity) as GameObject;
            p0.name = i.ToString()+" "+ this.GetComponent<Grid>().mode.ToString(); //numerujemy je
            p0.AddComponent<SpriteRenderer>().sprite = p; //nadajemy wyglad 
           
            p0.transform.parent = this.transform;
            GameObject Mother; //GameObject potrzebny do ustawienia pierwotnej pozycji cząstki
           
            if (this.GetComponent<Grid>().mode == Grid.Mode.Pozycja)   //Jeśli Grid ma tryb pęd- wybieramy  wartość z naszej listy Fc, jest ona listą wszystkich komórek siatki przestrzeni z pierwszej kolumny (tj. kolumny -r)
            {
                //sposób generowania- idziemy od środka, na zmianę na prawo i lewo od środka
if(i%2==0)                Mother = Fc[(N/2-1+ count / 2)%Fc.Count];
                else Mother = Fc[(System.Math.Abs(N/2-1 - count / 2))%Fc.Count];

                count++;
                if (count > Fc.Count) count -= Fc.Count;
            }
            else
            {
                //Jeśli Grid ma tryb pęd- wybieramy losową wartość z naszej listy Fc, jest ona listą wszystkich komórek siatki pedu
                Mother = Fc[Random.Range(0, Fc.Count)];


            }
            //ustawianie pozycji 
            p0.transform.localScale = Mother.transform.localScale * 0.5f;
            p0.transform.position = Mother.transform.position;
            Vector3 _pos = new Vector3(p0.transform.position.x, p0.transform.position.y, p0.transform.position.z - 1);
            p0.transform.position = _pos;


            if (this.GetComponent<Grid>().mode == Grid.Mode.Pozycja)
            {

                //Generowanie nadrzednych czastek
                GameObject P = Instantiate(projectile, new Vector2(0f, 0f), Quaternion.identity) as GameObject;
                P.AddComponent<ParticleScript>();
                P.name = i.ToString();
                P.transform.SetParent(Part);
                //Jeśli tryb to Pozycja- do ParticleScript przekazujemy GameObjcet Poz i współrzędne poz
                P.GetComponent<ParticleScript>().Pos = p0;
                P.GetComponent<ParticleScript>().posF[0] =(float) Mother.GetComponent<CellScript>().pos[1];
                P.GetComponent<ParticleScript>().posF[1] = (float)Mother.GetComponent<CellScript>().pos[0];
            }
            else
            {

                //Jeśli tryb to ped- do ParticleScript przekazujemy GameObjcet Ped i współrzędne ped
                GameObject.Find(i.ToString()).GetComponent<ParticleScript>().Ped = p0;
                GameObject.Find(i.ToString()).GetComponent<ParticleScript>().ped = Mother.GetComponent<CellScript>().pos;
            }

        }



        Destroy(projectile);
	}
	

}
