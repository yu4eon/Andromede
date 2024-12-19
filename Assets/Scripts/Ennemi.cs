// =======================================
//     Auteur: Léon Yu
//     Automne 2023, TIM
// =======================================

using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

/// <summary>
/// Utilisé pour les ennemis, incluant leurs mouvement
/// et intéractions
/// </summary>
public class Ennemi : MonoBehaviour
{
    // --- Champs --------------------------------------------------------------

    [SerializeField] private Sprite[] _spriteEnnemis; //Tableau de sprites du vaisseau ennemis
    [SerializeField] private GameObject _prefabPointsBoni; //prefab de l'animation de pointage
    [SerializeField] private GameObject _prefabExplosion; //prefab de l'Explosion lors qu'il se fait détruire
    [SerializeField] private GameObject _prefabLaser; //prefab de laser, différent de celle du joueur
    [SerializeField] private GameObject _pointLaser; //point d'où le laser est tiré
    [SerializeField] private AudioClip _sonExplosion; //Son d'explosion lors de sa destruction
    [SerializeField] private AudioClip _sonLaser; //son joué lorsque l'ennemi tire

    private float _vitessex = 2f; //La vitesse du vaisseau en x
    private float _vitessey = 1.2f;//La vitesse du vaisseau en y
    private int _directiony = 1; //Permet de dire au vaisseau d'aller vers le bas ou le haut
    private float _limiteMouvementHaut; //Détermine la limite du mouvement du vaisseau en y vers le haut
    private float _limiteMouvementBas; //Détermine la limite du mouvement du vaisseau en y vers le bas
    private float _amplitudeMouvementy = 1f; //Détermine la distance en y que le vaisseau peut aller avant de 'rebondir'
    private float _limitex = 9f; //Limite de la scène en x
    private float _limitey = 4f; //Limite de la scène en y
    private float _delaiLaser = 2f; //À quel intervalle l'ennemi tire

    private SpriteRenderer _sr; //Afin de changer aléatoirement le sprite du vaisseau

    // --- Initialisation -------------------------------------------------------

    /// <summary>
    /// S'exécute avant la mise à jour du premier frame
    /// </summary>
    void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
        Init();        
    }

    // --- Méthodes Privées -------------------------------------------------------

    /// <summary>
    /// S'exécute à tous les frames
    /// </summary>
    private void Update()
    {
        Bouger();
    }

    /// <summary>
    /// Permet à l'ennemi de bouger en x et en y, presque
    /// comme les aliens dans space invaders mais à l'horizontal
    /// </summary>
    private void Bouger()
    {

        Vector3 v = new Vector3(-1 * _vitessex, _directiony * _vitessey , 0);

        transform.Translate(v * Time.deltaTime, Space.World);

        if(transform.position.y >= _limiteMouvementHaut 
            || transform.position.y <= _limiteMouvementBas)
        {
            _directiony = -_directiony;
        }

        if (transform.position.x < -_limitex) Destroy(gameObject);
    }

    /// <summary>
    /// L'initialisation de l'ennemi et son trajet 
    /// possible et commence de tirer.
    /// </summary>
    private void Init()
    {
        int sprite = Random.Range(0, _spriteEnnemis.Length);
        _sr.sprite = _spriteEnnemis[sprite];
        float randomy = Random.Range(-_limitey, _limitey);
        transform.position = new Vector3(_limitex, randomy, 0);
        InvokeRepeating("Tirer", 0, _delaiLaser);
        _limiteMouvementHaut = Mathf.Clamp(transform.position.y + _amplitudeMouvementy, -_limitey, _limitey);
        _limiteMouvementBas = Mathf.Clamp(transform.position.y - _amplitudeMouvementy, -_limitey, _limitey);
    }

    /// <summary>
    /// Vérifie les collisions avec le joueur
    /// </summary>
    /// <param name="coll">L'objet avec qui le gameObject est rentré en contact</param>
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            Detruire();
            coll.gameObject.GetComponent<Vaisseau>().PerdreVies();
            //Vaisseau.instance.PerdreVies();
        }
    }

    /// <summary>
    /// Instantie un laser dangeureux pour le joueur, est
    /// appelé à répétition selon le délai
    /// </summary>
    private void Tirer()
    {
        Quaternion rotationLaser = new Quaternion(0f, 0f, 90f, 90f);
        SoundManager.instance.JouerSon(_sonLaser);
        Instantiate(_prefabLaser, _pointLaser.transform.position, rotationLaser);
    }

    // --- Méthodes Publiques -------------------------------------------------------

    /// <summary>
    /// Appelé lorsque l'ennemi est détruite par collision
    /// de laser ou du joueur et ajoute des points.
    /// </summary>
    public void Detruire()
    {
        Instantiate(_prefabPointsBoni, transform.position, Quaternion.identity);
        GameManager.Instance.AjouterPointsBonus(100);
        Instantiate(_prefabExplosion, transform.position, Quaternion.identity);
        SoundManager.instance.JouerSon(_sonExplosion);
        Destroy(gameObject);
    }
}
