// =======================================
//     Auteur: L�on Yu
//     Automne 2023, TIM
// =======================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Utilis� pour le d�filement du background
/// </summary>
public class Defilement : MonoBehaviour
{
    // --- Champs --------------------------------------------------------------

    private float _vitesseDefilement = 1f; //vitesse de d�filement de la sc�ne

    private float _largeurBackground; //la largeur de l'image pour d�terminer quand r�p�ter
    private Vector3 _posDepart; //la position de d�part de l'image

    private float _posX = 0; //la position de l'image en x

    // --- Initialisation -------------------------------------------------------

    /// <summary>
    /// S'ex�cute avant la mise � jour du premier frame
    /// </summary>
    void Start()
    {
        _largeurBackground = gameObject.GetComponent<SpriteRenderer>().bounds.extents.x * 2;
        _posX = -_largeurBackground / 2;
        _posDepart = transform.position;
    }

    // --- M�thodes Priv�es -------------------------------------------------------

    /// <summary>
    /// S'ex�cute � tous les frames
    /// Le background d�file vers la gauche selon
    /// la vitesse et se r�p�te une fois rendu � la limite.
    /// </summary>

    void Update()
    {
        _posX += Time.deltaTime * _vitesseDefilement;
        float _nouvellePos = Mathf.Repeat(-_posX, _largeurBackground);
        transform.position = _posDepart + Vector3.right * _nouvellePos;
    }

}
