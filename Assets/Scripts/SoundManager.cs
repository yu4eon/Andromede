// =======================================
//     Auteur: Léon Yu
//     Automne 2023, TIM
// =======================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Utilisé pour la gestion des sons.
/// Appelé pour jouer des sons.
/// </summary>
public class SoundManager : MonoBehaviour
{
    // --- Champs --------------------------------------------------------------

    private AudioSource _audioSource; //l'audioSource du Soundmanager
    private static SoundManager _instance; //singleton
    public static SoundManager instance
    {
        get { return _instance; }
    }

    // --- Initialisation -------------------------------------------------------

    /// <summary>
    /// S'exécute avant le Start
    /// </summary>
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    /// <summary>
    /// S'exécute avant la mise à jour du premier frame
    /// </summary>
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        _audioSource = GetComponent<AudioSource>();
    }

    // --- Méthodes Publiques -------------------------------------------------------

    /// <summary>
    /// Fonction appelé pour jouer un son
    /// </summary>
    public void JouerSon(AudioClip son, float volume = 1)
    {
        _audioSource.PlayOneShot(son, volume);
    }
}
