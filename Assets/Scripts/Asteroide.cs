// =======================================
//     Auteur: L�on Yu
//     Automne 2023, TIM
// =======================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Utilis� par les ast�roides et leur initialisation
/// </summary>
public class Asteroide : MonoBehaviour
{
    // --- Champs --------------------------------------------------------------

    [SerializeField] private Sprite[] _spriteAsteroides; //Tableau de tous les sprites d'ast�roides
    [SerializeField] private GameObject _prefabExplosionBrun; //Prefab de l'Explosion version brun
    [SerializeField] private GameObject _prefabExplosionGris; //Prefab de l'Explosion version grise
    [SerializeField] private GameObject _prefabPoints; // prefab de l'effet qui ajoute des points lors de la d�struction d'un ast�roide
    [SerializeField] private AudioClip _sonExplosion; //son de l'ast�roide lorsqu'elle est d�truite
    private float _limitex = 9f; //Limite de la sc�ne en x
    private float _limitey = 5f; //Limite de la sc�ne en y
    private float _vitesse; //Vitesse de l'ast�roide
    private float _vitesseRotation; //Vitesse de rotation
    private SpriteRenderer _sr; //utilis� pour changer son sprite de fa�on al�atoire
    private int _couleur; //Utiliser pour d�terminer la couleur de l'ast�roide � partir du num�ro de sprite

    // --- Initialisation -------------------------------------------------------

    /// <summary>
    /// S'ex�cute avant la mise � jour du premier frame
    /// </summary>
    void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
        Recycler();
    }

    // --- M�thodes Priv�es -------------------------------------------------------

    /// <summary>
    /// S'ex�cute � tous les frames
    /// </summary>
    void Update()
    {
        Bouger();
    }

    /// <summary>
    /// Permet � l'ast�roide de bouger, de tourner et de se
    /// recycler une fois rendu � la fin de la sc�ne
    /// </summary>
    private void Bouger()
    {
        transform.Translate(Vector3.left * Time.deltaTime * _vitesse, Space.World);
        if (transform.position.x < -_limitex) Recycler();
        transform.Rotate(0, 0, _vitesseRotation*Time.deltaTime);
    }

    /// <summary>
    /// R�inisialise l'ast�roide al�atoirement, et utilis� afin de d�terminer
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
    /// V�rifie les collisions avec le joueur et le fait perdre des vies
    /// </summary>
    /// <param name="coll">L'objet avec qui le gameObject est rentr� en contact</param>
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if( coll.CompareTag("Player"))
        {
            Detruire();
            coll.gameObject.GetComponent<Vaisseau>().PerdreVies();
        }
    }

    // --- M�thodes Publiques -------------------------------------------------------

    /// <summary>
    /// permet � l'ast�roide de se d�truire et d'ajouter des points. 
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
