// =======================================
//     Auteur: Léon Yu
//     Automne 2023, TIM
// =======================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Utilisé par le laser du joueur pour son mouvement
/// et ses intéractions avec les obstacles
/// </summary>
public class Laser : MonoBehaviour
{
    // --- Champs --------------------------------------------------------------
    private float _vitesse = 10f; //la vitesse du laser
    private float _limitex = 9f; //Limite de la scène en x

    // --- Méthodes Privées -------------------------------------------------------

    /// <summary>
    /// S'exécute à tous les frames
    /// </summary>
    private void Update()
    {
        Bouger();
    }

    /// <summary>
    /// Permet au laser de bouger vers la droite
    /// </summary>
    private void Bouger()
    {
        transform.Translate(Vector3.right * Time.deltaTime * _vitesse, Space.World);
        if (transform.position.x > _limitex)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Détecte les collisions et réagit selon leur tag
    /// </summary>
    /// <param name="coll">L'objet avec qui le gameObject est rentré en contact</param>
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Danger"))
        {
            Destroy(gameObject);
            coll.GetComponent<Asteroide>().Detruire();
        }
        if (coll.CompareTag("Ennemi"))
        {
            Destroy(gameObject);
            coll.GetComponent<Ennemi>().Detruire();
        }
    }

}
