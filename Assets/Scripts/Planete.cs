// =======================================
//     Auteur: Léon Yu
//     Automne 2023, TIM
// =======================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Utilisé par les planètes pour le défilement
/// leur initialisation
/// </summary>
public class Planete : MonoBehaviour
{
    // --- Champs --------------------------------------------------------------

    [SerializeField] private Sprite[] _spritesPlanetes; //tableau des sprites différents des planètes
    private float _limitex = 12f; //Limite de la scène en x
    private float _limitey = 5f; //Limite de la scène en y
    private float _vitesse = 2.5f; //Vitesse de la planète
    private SpriteRenderer _sr; //pour changer aléatoirement le sprite

    // --- Initialisation -------------------------------------------------------

    /// <summary>
    /// S'exécute avant la mise à jour du premier frame
    /// </summary>
    void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
        float randomy = Random.Range(-_limitey, _limitey);
        float ratio = Random.Range(0.1f, 0.4f);
        float rotation = Random.Range(-90f, 90f);
        _sr.sprite = _spritesPlanetes[Random.Range(0, _spritesPlanetes.Length)];
        transform.localScale = new Vector3(ratio, ratio, ratio);
        transform.rotation = Quaternion.Euler(0, 0, rotation);
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
    /// Permet aux planètes de bouger et de se recyclé
    /// une fois la limite atteinte
    /// </summary>
    private void Bouger()
    {
        transform.Translate(Vector3.left * Time.deltaTime * _vitesse, Space.World);
        if (transform.position.x < -_limitex) Recycler();
    }

    /// <summary>
    /// Réinitialise la planète avec des caractéristiques
    /// aléatoires
    /// </summary>
    private void Recycler()
    {
        float randomy = Random.Range(-_limitey, _limitey);
        float ratio = Random.Range(0.1f, 0.4f);
        float rotation = Random.Range(-90f, 90f);
        _sr.sprite = _spritesPlanetes[Random.Range(0, _spritesPlanetes.Length)];
        transform.localScale = new Vector3(ratio, ratio, ratio);
        transform.rotation = Quaternion.Euler(0, 0, rotation);
        transform.position = new Vector3(_limitex, randomy, 0);
    }

}
