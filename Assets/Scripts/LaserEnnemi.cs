// =======================================
//     Auteur: Léon Yu
//     Automne 2023, TIM
// =======================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Utilisé par le laser de l'ennemi pour son mouvement
/// et l'intéraction avec le joueur
/// </summary>
public class LaserEnnemi : MonoBehaviour
{
    // --- Champs --------------------------------------------------------------

    private float _vitesse = 10f; //la vitesse du laser
    private float _limitex = 9f; //Limite de la scène en x

    // --- Méthodes Privées -------------------------------------------------------

    /// <summary>
    /// S'exécute à tous les frames
    /// </summary>
    void Update()
    {
        Bouger();
    }

    /// <summary>
    /// Permet au laser de bouger vers la droite
    /// </summary>
    private void Bouger()
    {
        transform.Translate(Vector3.left * Time.deltaTime * _vitesse, Space.World);
        if (transform.position.x < -_limitex)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Détecte les collisions avec le joueur
    /// </summary>
    /// <param name="coll">L'objet avec qui le gameObject est rentré en contact</param>
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            Destroy(gameObject);
            coll.GetComponent<Vaisseau>().PerdreVies();
        }
    }

}
