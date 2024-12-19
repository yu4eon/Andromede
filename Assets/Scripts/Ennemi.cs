// =======================================
//     Auteur: L�on Yu
//     Automne 2023, TIM
// =======================================

using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

/// <summary>
/// Utilis� pour les ennemis, incluant leurs mouvement
/// et int�ractions
/// </summary>
public class Ennemi : MonoBehaviour
{
    // --- Champs --------------------------------------------------------------

    [SerializeField] private Sprite[] _spriteEnnemis; //Tableau de sprites du vaisseau ennemis
    [SerializeField] private GameObject _prefabPointsBoni; //prefab de l'animation de pointage
    [SerializeField] private GameObject _prefabExplosion; //prefab de l'Explosion lors qu'il se fait d�truire
    [SerializeField] private GameObject _prefabLaser; //prefab de laser, diff�rent de celle du joueur
    [SerializeField] private GameObject _pointLaser; //point d'o� le laser est tir�
    [SerializeField] private AudioClip _sonExplosion; //Son d'explosion lors de sa destruction
    [SerializeField] private AudioClip _sonLaser; //son jou� lorsque l'ennemi tire

    private float _vitessex = 2f; //La vitesse du vaisseau en x
    private float _vitessey = 1.2f;//La vitesse du vaisseau en y
    private int _directiony = 1; //Permet de dire au vaisseau d'aller vers le bas ou le haut
    private float _limiteMouvementHaut; //D�termine la limite du mouvement du vaisseau en y vers le haut
    private float _limiteMouvementBas; //D�termine la limite du mouvement du vaisseau en y vers le bas
    private float _amplitudeMouvementy = 1f; //D�termine la distance en y que le vaisseau peut aller avant de 'rebondir'
    private float _limitex = 9f; //Limite de la sc�ne en x
    private float _limitey = 4f; //Limite de la sc�ne en y
    private float _delaiLaser = 2f; //� quel intervalle l'ennemi tire

    private SpriteRenderer _sr; //Afin de changer al�atoirement le sprite du vaisseau

    // --- Initialisation -------------------------------------------------------

    /// <summary>
    /// S'ex�cute avant la mise � jour du premier frame
    /// </summary>
    void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
        Init();        
    }

    // --- M�thodes Priv�es -------------------------------------------------------

    /// <summary>
    /// S'ex�cute � tous les frames
    /// </summary>
    private void Update()
    {
        Bouger();
    }

    /// <summary>
    /// Permet � l'ennemi de bouger en x et en y, presque
    /// comme les aliens dans space invaders mais � l'horizontal
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
    /// V�rifie les collisions avec le joueur
    /// </summary>
    /// <param name="coll">L'objet avec qui le gameObject est rentr� en contact</param>
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
    /// appel� � r�p�tition selon le d�lai
    /// </summary>
    private void Tirer()
    {
        Quaternion rotationLaser = new Quaternion(0f, 0f, 90f, 90f);
        SoundManager.instance.JouerSon(_sonLaser);
        Instantiate(_prefabLaser, _pointLaser.transform.position, rotationLaser);
    }

    // --- M�thodes Publiques -------------------------------------------------------

    /// <summary>
    /// Appel� lorsque l'ennemi est d�truite par collision
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
