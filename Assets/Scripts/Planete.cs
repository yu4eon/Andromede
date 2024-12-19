// =======================================
//     Auteur: L�on Yu
//     Automne 2023, TIM
// =======================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Utilis� par les plan�tes pour le d�filement
/// leur initialisation
/// </summary>
public class Planete : MonoBehaviour
{
    // --- Champs --------------------------------------------------------------

    [SerializeField] private Sprite[] _spritesPlanetes; //tableau des sprites diff�rents des plan�tes
    private float _limitex = 12f; //Limite de la sc�ne en x
    private float _limitey = 5f; //Limite de la sc�ne en y
    private float _vitesse = 2.5f; //Vitesse de la plan�te
    private SpriteRenderer _sr; //pour changer al�atoirement le sprite

    // --- Initialisation -------------------------------------------------------

    /// <summary>
    /// S'ex�cute avant la mise � jour du premier frame
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

    // --- M�thodes Priv�es -------------------------------------------------------

    /// <summary>
    /// S'ex�cute � tous les frames
    /// </summary>
    void Update()
    {
        Bouger();
    }

    /// <summary>
    /// Permet aux plan�tes de bouger et de se recycl�
    /// une fois la limite atteinte
    /// </summary>
    private void Bouger()
    {
        transform.Translate(Vector3.left * Time.deltaTime * _vitesse, Space.World);
        if (transform.position.x < -_limitex) Recycler();
    }

    /// <summary>
    /// R�initialise la plan�te avec des caract�ristiques
    /// al�atoires
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
