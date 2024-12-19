// =======================================
//     Auteur: Léon Yu
//     Automne 2023, TIM
// =======================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gère le temps avant que le vaisseau puisse tirer à nouveau
/// </summary>
public class BarreRecharge : MonoBehaviour
{
    // --- Champs --------------------------------------------------------------

    [SerializeField] private Vaisseau _vaisseau; //Appelle au vaisseau pour lui dire qu'il peut tirer à nouveau

    // --- Initialisation -------------------------------------------------------

    /// <summary>
    /// S'exécute avant la mise à jour du premier frame
    /// </summary>
    private void Start()
    {
        _vaisseau = _vaisseau.GetComponent<Vaisseau>();
    }

    // --- Méthodes Publiques -------------------------------------------------------

    /// <summary>
    /// Appelé par une event d'animation, qui se désactive
    /// à la fin et qui dit au vaisseau qu'il peut tirer.
    /// </summary>
    public void Desactiver()
    {
        _vaisseau.Recharger();
        gameObject.SetActive(false);
    }
}
