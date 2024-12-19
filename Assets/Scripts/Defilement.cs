// =======================================
//     Auteur: Léon Yu
//     Automne 2023, TIM
// =======================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Utilisé pour le défilement du background
/// </summary>
public class Defilement : MonoBehaviour
{
    // --- Champs --------------------------------------------------------------

    private float _vitesseDefilement = 1f; //vitesse de défilement de la scène

    private float _largeurBackground; //la largeur de l'image pour déterminer quand répéter
    private Vector3 _posDepart; //la position de départ de l'image

    private float _posX = 0; //la position de l'image en x

    // --- Initialisation -------------------------------------------------------

    /// <summary>
    /// S'exécute avant la mise à jour du premier frame
    /// </summary>
    void Start()
    {
        _largeurBackground = gameObject.GetComponent<SpriteRenderer>().bounds.extents.x * 2;
        _posX = -_largeurBackground / 2;
        _posDepart = transform.position;
    }

    // --- Méthodes Privées -------------------------------------------------------

    /// <summary>
    /// S'exécute à tous les frames
    /// Le background défile vers la gauche selon
    /// la vitesse et se répète une fois rendu à la limite.
    /// </summary>

    void Update()
    {
        _posX += Time.deltaTime * _vitesseDefilement;
        float _nouvellePos = Mathf.Repeat(-_posX, _largeurBackground);
        transform.position = _posDepart + Vector3.right * _nouvellePos;
    }

}
