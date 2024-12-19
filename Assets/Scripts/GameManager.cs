// =======================================
//     Auteur: Léon Yu
//     Automne 2023, TIM
// =======================================

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Gestion du jeu en général, incluant les obstacles
/// et le pointage
/// </summary>
public class GameManager : MonoBehaviour
{
    // --- Champs --------------------------------------------------------------
    [SerializeField] private GameObject _asteroide; //Prefab de l'asteroide, instantié progressivement
    [SerializeField] private TextMeshProUGUI _champScore; //Le champ qui affiche le pointage du joueur
    [SerializeField] private GameObject _prefabPowerup; //prefab des powerups, instantié selon un compteur
    [SerializeField] private GameObject _prefabEnnemi;  //prefab des ennemis, instantié selon un compteur
    private int _nbAsteroides; //Nombre d'astéroides possible sur la scène
    private GameObject _groupeAsteroides; //GameObject vide pour regrouper les astéroides
    private int _indexAsteroides; //Permet de compter le nombre d'astéroides présentement sur la scène
    private static int _score; //Le score du joueur, réinitialisé lorsque le joueur revient au jeu
    private float _vitesseScore = 0.2f; //La vitesse à laquelle le joueur accumule des points
    private int _compteurPowerup; //Compteur pour les powerups, augmenté selon le nombre d'astéroide initialisé et recyclé
    private int _compteurPowerupMax = 15; //Quand le compteur atteind le max, un powerup est instantié
    private int _compteurEnnemi; //Compteur pour les ennemis, augmenté selon le nombre d'astéroide initialisé et recyclé
    private int _compteurEnnemiMax = 25; //Quand le compteur atteind le max, un ennmi est instantié
    private static GameManager _instance; //Singleton
    public static GameManager Instance
    {  
        get { return _instance; } 
    }

    // --- Initialisation -------------------------------------------------------

    /// <summary>
    /// S'exécute avant le Start
    /// </summary>
    private void Awake()
    {
         _instance = this;
    }

    /// <summary>
    /// S'exécute avant la mise à jour du premier frame
    /// Affiche le score, réinitialise le score si on est sur la scène jeu
    /// initialisation des astéroides et accumulation du pointage
    /// </summary>
    void Start()
    {
        Time.timeScale = 1f;
        if (SceneManager.GetActiveScene().name == "Fin")
        {
            _champScore.text = "Score : " + _score;
        }
        
        if (SceneManager.GetActiveScene().name != "Jeu") return;
        _score = 0;
        _groupeAsteroides = new GameObject("GroupeAsteroides");
        _nbAsteroides = 8;
        InvokeRepeating("CreerAsteroides", 0f, 1f);
        InvokeRepeating("AjouterPoints", 0f, _vitesseScore);
    }

    // --- Méthodes Privées -------------------------------------------------------

    /// <summary>
    ///Change la scène à la fin
    /// </summary>
    private void AllerFin()
    {
        SceneManager.LoadScene("Fin");
    }

    /// <summary>
    /// Instantie les asteroides une à une
    /// </summary>
    private void CreerAsteroides()
    {
        GameObject obj = null;
        _indexAsteroides++;
        if( _indexAsteroides > _nbAsteroides )
        {
            CancelInvoke("CreerAsteroides");
            return;
        }
        obj = Instantiate(_asteroide, Vector3.zero, Quaternion.identity);
        obj.transform.parent = _groupeAsteroides.transform;
    }

    /// <summary>
    /// Ajoute 1 point à une intervalle 
    /// selon la vitesse du score
    /// </summary>
    private void AjouterPoints()
    {
        _score += 1;
        _champScore.text = "score : " + _score;  
    }

    // --- Méthodes Publiques -------------------------------------------------------

    /// <summary>
    /// Activé lorsque le joueur n'as plus de vies
    /// </summary>
    public void GameOver()
    {
        Invoke("AllerFin", 1.5f);
        CancelInvoke("AjouterPoints");
    }

    /// <summary>
    /// Augmente de compteur de powerup et
    /// instantie un powerup lorsqu'elle atteint le max
    /// </summary>
    public void AjouterCompteur()
    {
        _compteurPowerup++;
        if(_compteurPowerup >= _compteurPowerupMax)
        {
            Instantiate(_prefabPowerup, Vector3.zero, Quaternion.identity);
            _compteurPowerup = 0;
        }
    }

    /// <summary>
    /// Augmente de compteur d'ennemi et
    /// instantie un ennemi lorsqu'elle atteint le max
    /// </summary>
    public void AjouterCompteurEnnemi()
    {
        _compteurEnnemi++;
        if (_compteurEnnemi >= _compteurEnnemiMax)
        {
            Quaternion rotationEnnemi = new Quaternion(0f, 0f, -90f, 90f);
            Instantiate(_prefabEnnemi, Vector3.zero, rotationEnnemi);
            _compteurEnnemi = 0;
        }
    }

    /// <summary>
    ///Ajoute des points bonus
    ///
    ///J'ai voulu que cette fonction soit celle de AjouterPoints mais Invoke ne semble
    ///pas marcher avec des paramètres.
    /// </summary>
    /// <param name="points">Le nombre de points que l'on veut ajouter</param>

    public void AjouterPointsBonus(int points)
    {
        _score += points;
        _champScore.text = "score : " + _score;
    }

    /// <summary>
    /// Arrête l'invokeRepeating pour l'ajout des points
    /// </summary>
    public void ArreterPointage()
    {
        CancelInvoke("AjouterPoints");
    }

    /// <summary>
    /// Résume l'invokeRepeating pour l'ajout des points
    /// </summary>
    public void ResumerPointage()
    {
        InvokeRepeating("AjouterPoints", 0, _vitesseScore);
    }

    
    
}