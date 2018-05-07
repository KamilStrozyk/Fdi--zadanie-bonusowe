using UnityEngine;
using System.Collections;
using UnityEngine.UI;
//skrypt głowny, służacy generowaniu graficznej siatki i uruchamianiu reszty skry[tw
public class Grid : MonoBehaviour
{
    //Nasze zmienne 
    public InputField inf;
    public int r;
    private int R;
    //==============================
    //Specyfikacja siatki: rozmiar, odstep miedzy komorkami, grafika komorki, rozmiar i skala komorki (ustawiane automatycznie)
    [SerializeField]
    private Vector2 gridSize;
    [SerializeField]
    private Vector2 gridOffset;
    [SerializeField]
    private Sprite cellSprite;
    private Vector2 cellSize;
    private Vector2 cellScale;
    //===============================
    private MainParticleScript MPS;
    [SerializeField]
    //ustawianie trybu skryptu
    public enum Mode
    {
        Pozycja, Ped

    }
    public Mode mode;
    void Start() //funkcja uruchamiana na samym początku działania programu
    {
        //tworzymy połączenie do  MainParticleScript (w skrócie MPS)
        MPS = GameObject.Find("Czastki").GetComponent<MainParticleScript>();
    }

   public void InitCells() //funckja rozpoczynajaca generowanie, wywoływana na przycisk
    {
        r = int.Parse(inf.text); //pobieramy r z okienka
        if (mode == Mode.Pozycja) R = 2 * r + 1;
        else R = r +1 -r%2; //jesli tryb skryptu to ped, to R=P


        //================================================
        //parametry pojedynczej komorki
        GameObject cellObject = new GameObject();
        cellObject.AddComponent<SpriteRenderer>().sprite = cellSprite;
        cellSize = cellSprite.bounds.size;
        Vector2 newCellSize = new Vector2(gridSize.x / (float)R, gridSize.y / (float)R);
        cellScale.x = newCellSize.x / cellSize.x;
        cellScale.y = newCellSize.y / cellSize.y;
        cellSize = newCellSize; 
        cellObject.transform.localScale = new Vector2(cellScale.x, cellScale.y);
        gridOffset.x = -(gridSize.x / 2) + cellSize.x / 2;
        gridOffset.y = -(gridSize.y / 2) + cellSize.y / 2;
        //==================================================
        //MPS- w zależności od trybu skryptu, generuje w MPS nową tablicę dwuwymiarową dla siatki przestrzeni lub siatki pędu
        if (mode == Mode.Pozycja) MPS.pos = new Transform[R, R];
        else MPS.ped = new Transform[R, R];
           //główna pętla tworząca komórki
            for (int row = 0; row < R; row++)
        {
            for (int col = 0; col < R; col++)
            {
                //ustawianie pozycji
                Vector2 pos = new Vector2(col * cellSize.x + gridOffset.x + transform.position.x, row * cellSize.y + gridOffset.y + transform.position.y);

                //Generowanie i dodawanie CellScript
                GameObject c0 = Instantiate(cellObject, pos, Quaternion.identity) as GameObject;
                c0.AddComponent<CellScript>();
                //generowanie odpowiedniego indeksu w zależności od typu siatki
                if (mode == Mode.Pozycja)
                {
                    c0.GetComponent<CellScript>().pos[0] = (r * (-1) + col);
                    c0.GetComponent<CellScript>().pos[1] = (r * (-1) + row);
                }
                else
                {
                    c0.GetComponent<CellScript>().pos[0] = (r/2 * (-1) + col);
                    c0.GetComponent<CellScript>().pos[1] = (r/2 * (-1) + row);


                }
                c0.name = "[" + c0.GetComponent<CellScript>().pos[0].ToString() + ";" + c0.GetComponent<CellScript>().pos[1].ToString() + "]"; //generuje nazwę obiektu, przydatne w edytorze
                if (mode == Mode.Pozycja) MPS.pos[row, col] = c0.transform;
                else MPS.ped[row, col] = c0.transform;
                c0.transform.parent = transform;
                if (col == 0 && mode==Mode.Pozycja) this.GetComponent<MakeParticles>().Fc.Add(c0); //do ustawiania równomiernie w pierwszej kolumnie w przestrzeni  (tj. kolumny -r)
                else if(mode==Mode.Ped) this.GetComponent<MakeParticles>().Fc.Add(c0); //do losowych pedów
            }
        }

        Destroy(cellObject);
        GetComponent<MakeParticles>().Init(); //wywolanie powstawania czastek
        if (mode == Mode.Ped) MPS.Init(r); //wywolanie generowania czasteczek
    }

    //Pomocnicze do edytora- zobaczenie wielkoci siatki
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, gridSize);
    }
}