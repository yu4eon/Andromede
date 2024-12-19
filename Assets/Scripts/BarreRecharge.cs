// =======================================
//     Auteur: L�on Yu
//     Automne 2023, TIM
// =======================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// G�re le temps avant que le vaisseau puisse tirer � nouveau
/// </summary>
public class BarreRecharge : MonoBehaviour
{
    // --- Champs --------------------------------------------------------------

    [SerializeField] private Vaisseau _vaisseau; //Appelle au vaisseau pour lui dire qu'il peut tirer � nouveau

    // --- Initialisation -------------------------------------------------------

    /// <summary>
    /// S'ex�cute avant la mise � jour du premier frame
    /// </summary>
    private void Start()
    {
        _vaisseau = _vaisseau.GetComponent<Vaisseau>();
    }

    // --- M�thodes Publiques -------------------------------------------------------

    /// <summary>
    /// Appel� par une event d'animation, qui se d�sactive
    /// � la fin et qui dit au vaisseau qu'il peut tirer.
    /// </summary>
    public void Desactiver()
    {
        _vaisseau.Recharger();
        gameObject.SetActive(false);
    }
}
