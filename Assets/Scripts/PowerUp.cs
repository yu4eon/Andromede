// =======================================
//     Auteur: L�on Yu
//     Automne 2023, TIM
// =======================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Utilis� pour d�terminer le type de powerup
/// et son initialisation
/// </summary>
public class PowerUp : MonoBehaviour
{
    // --- Champs --------------------------------------------------------------

    [SerializeField] private Sprite[] _spritesPowerups; //tableau des sprites de powerups
    [SerializeField] private GameObject _prefabPointsBoni; //prefab de l'effet d'ajout de points
    private float _limitex = 9f; //Limite de la sc�ne en x
    private float _limitey = 4.5f; //Limite de la sc�ne en y
    private float _vitesse; //Vitesse de l'ast�roide
    private float _vitesseRotation; //Vitesse de rotation
    private SpriteRenderer _sr; //pour changer le sprite al�atoirement
    private int _numeroPowerup; //afin de d�terminer quel sprite � �t� pris

    // --- Initialisation -------------------------------------------------------

    /// <summary>
    /// S'ex�cute avant la mise � jour du premier frame
    /// </summary>
    void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
        Spawn();
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
    /// Permet au powerup de bouger
    /// </summary>
    private void Bouger()
    {
        transform.Translate(Vector3.left * Time.deltaTime * _vitesse, Space.World);
        if (transform.position.x < -_limitex) Destroy(gameObject);
        transform.Rotate(0, 0, _vitesseRotation * Time.deltaTime);
    }

    /// <summary>
    /// Initialisation du powerup al�atoire
    /// </summary>
    private void Spawn()
    {
        _numeroPowerup = Random.Range(0, _spritesPowerups.Length);
        _sr.sprite = _spritesPowerups[_numeroPowerup];
        float randomy = Random.Range(-_limitey, _limitey);
        float rotation = Random.Range(0f, 90f);
        _vitesse = Random.Range(3f, 7f);
        _vitesseRotation = Random.Range(10f, 30f);
        if (Random.Range(0, 2) == 0) _vitesseRotation = -_vitesseRotation;
        transform.rotation = Quaternion.Euler(0, 0, rotation);
        transform.position = new Vector3(_limitex, randomy, 0);
    }

    /// <summary>
    /// V�rifie les collisions avec le joueur et lui envoie un int
    /// selon son sprite
    /// </summary>
    /// <param name="coll">L'objet avec qui le gameObject est rentr� en contact</param>
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            //Si le powerup s'agit de l'�toile, on instantie un effet aussi
            if(_numeroPowerup == 2)
            {
                Instantiate(_prefabPointsBoni, transform.position, Quaternion.identity);
            }
            Debug.Log("Le joueur a un powerup");
            coll.GetComponent<Vaisseau>().ObtenirPowerup(_numeroPowerup);
            Destroy(gameObject);
        }

        
    }
}
