// =======================================
//     Auteur: Léon Yu
//     Automne 2023, TIM
// =======================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Utilisé par les astéroides et leur initialisation
/// </summary>
public class Asteroide : MonoBehaviour
{
    // --- Champs --------------------------------------------------------------

    [SerializeField] private Sprite[] _spriteAsteroides; //Tableau de tous les sprites d'astéroides
    [SerializeField] private GameObject _prefabExplosionBrun; //Prefab de l'Explosion version brun
    [SerializeField] private GameObject _prefabExplosionGris; //Prefab de l'Explosion version grise
    [SerializeField] private GameObject _prefabPoints; // prefab de l'effet qui ajoute des points lors de la déstruction d'un astéroide
    [SerializeField] private AudioClip _sonExplosion; //son de l'astéroide lorsqu'elle est détruite
    private float _limitex = 9f; //Limite de la scène en x
    private float _limitey = 5f; //Limite de la scène en y
    private float _vitesse; //Vitesse de l'astéroide
    private float _vitesseRotation; //Vitesse de rotation
    private SpriteRenderer _sr; //utilisé pour changer son sprite de façon aléatoire
    private int _couleur; //Utiliser pour déterminer la couleur de l'astéroide à partir du numéro de sprite

    // --- Initialisation -------------------------------------------------------

    /// <summary>
    /// S'exécute avant la mise à jour du premier frame
    /// </summary>
    void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
        Recycler();
    }

    // --- Méthodes Privées -------------------------------------------------------

    /// <summary>
    /// S'exécute à tous les frames
    /// </summary>
    void Update()
    {
        Bouger();
    }

    /// <summary>
    /// Permet à l'astéroide de bouger, de tourner et de se
    /// recycler une fois rendu à la fin de la scène
    /// </summary>
    private void Bouger()
    {
        transform.Translate(Vector3.left * Time.deltaTime * _vitesse, Space.World);
        if (transform.position.x < -_limitex) Recycler();
        transform.Rotate(0, 0, _vitesseRotation*Time.deltaTime);
    }

    /// <summary>
    /// Réinisialise l'astéroide aléatoirement, et utilisé afin de déterminer
    /// le timing de l'inisialisation des powerups et des ennemis.
    /// </summary>
    private void Recycler()
    {
        GameManager.Instance.AjouterCompteur();
        GameManager.Instance.AjouterCompteurEnnemi();
        int sprite = Random.Range(0, _spriteAsteroides.Length);
        if (sprite <= 3) _couleur = 0;
        else _couleur = 1;
        _sr.sprite = _spriteAsteroides[sprite];
        float randomy = Random.Range(-_limitey, _limitey);
        float ratio = Random.Range(0.8f, 2f);
        float rotation = Random.Range(0f, 90f);
        _vitesse = Random.Range(3f, 7f);
        _vitesseRotation = Random.Range(10f, 30f);
        if (Random.Range(0, 2) == 0) _vitesseRotation = -_vitesseRotation;
        transform.localScale = new Vector3(ratio, ratio, ratio);
        transform.rotation = Quaternion.Euler(0, 0, rotation);
        transform.position = new Vector3(_limitex, randomy, 0);
    }

    /// <summary>
    /// Vérifie les collisions avec le joueur et le fait perdre des vies
    /// </summary>
    /// <param name="coll">L'objet avec qui le gameObject est rentré en contact</param>
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if( coll.CompareTag("Player"))
        {
            Detruire();
            coll.gameObject.GetComponent<Vaisseau>().PerdreVies();
        }
    }

    // --- Méthodes Publiques -------------------------------------------------------

    /// <summary>
    /// permet à l'astéroide de se détruire et d'ajouter des points. 
    /// </summary>
    public void Detruire()
    {
        SoundManager.instance.JouerSon(_sonExplosion);
        GameManager.Instance.AjouterPointsBonus(10);
        Instantiate(_prefabPoints, transform.position, Quaternion.identity);

        if (_couleur == 0)
        {
            Instantiate(_prefabExplosionBrun, transform.position, transform.rotation);
        }
        else
        {
            Instantiate(_prefabExplosionGris, transform.position, transform.rotation);
        }
        Recycler();
    }
}
