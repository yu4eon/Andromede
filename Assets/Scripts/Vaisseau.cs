// =======================================
//     Auteur: L�on Yu
//     Automne 2023, TIM
// =======================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Utilis� par le vaisseau dans tous ses int�ractions
/// </summary>
public class Vaisseau : MonoBehaviour
{
    // --- Champs --------------------------------------------------------------

    [SerializeField] private GameObject[] _coeurs; //Tableau des vies du joueurs dans le UI
    [SerializeField] private GameObject _prefabExplosion; //prefab de l'explosion lorsque le joueur meurt
    [SerializeField] private GameObject _prefabLaser; //prefab du laser lorsque le joueur tire
    [SerializeField] private GameObject _pointLaser; //le point par o� le laser est instanti�
    [SerializeField] private GameObject _barreRecharge; //la barre de recharge pour mettre un cooldown au tir
    [SerializeField] private GameObject _bouclier; //le bouclier qui est activ� lorsque son powerup est ramass�
    [SerializeField] private GameObject _flame; //le flame de l'engin qui est activ� lorsque son powerup est ramass�

    [SerializeField] private AudioClip _sonBouclier; //le son lorsque le bouclier est activ�
    [SerializeField] private AudioClip _sonBouclierBriser; //le son lorsque le bouclier est bris�
    [SerializeField] private AudioClip _sonVitesse; //le son lorsqu'on ramasse le powerup de vitesse
    [SerializeField] private AudioClip _sonBonus; //le son lorsqu'on ramasse la bo�te de points (�toile)
    [SerializeField] private AudioClip _sonExplosion; //le son lorsque le vaisseau explose
    [SerializeField] private AudioClip _sonLaser; //le son du laser lors du tir
    [SerializeField] private AudioClip _sonRespawn; //le son jou� lorsque le joueur respawn

    private float _vitesse = 7f; //La vitesse du vaisseau
    private float _vitesseInitial; //La vitesse initale du vaissea
    private float _bonusVitesse = 3f; //le bonus de vitesse que donne le powerup

    private int _nbVies; //Nombre de vies du joeur d�pendant du nombre de coeurs
    private float _limitex = 6.9f; //Limite de la sc�ne en y
    private float _limitey = 3.9f; //Limite de la sc�ne en x
    private Vector3 _positionDepart; //la position de d�part pour le point de respawn

    private Collider2D _collision; //Pour activer o� d�sactiv� les collisions dans les fonctions
    private Animator _animator; //Pour jou� certaines animations

    private bool _enVie = true; //d�termine si le joueur est en vie
    private bool _peutTirer = true; //d�termine si le joueur peut tirer des lasers
    private bool _AUnBouclier = false; //d�termine si le bouclier est actif

    // --- Initialisation -------------------------------------------------------

    /// <summary>
    /// S'ex�cute avant la mise � jour du premier frame
    /// </summary>
    void Start()
    {
        _vitesseInitial = _vitesse;
        _positionDepart = transform.position;
        _nbVies = _coeurs.Length;   
        _collision = GetComponent<Collider2D>();
        _animator = GetComponent<Animator>();
    }

    // --- M�thodes Priv�es -------------------------------------------------------

    /// <summary>
    /// S'ex�cute � tous les frames
    /// </summary>
    void Update()
    {
        Bouger();
        Tirer();
    }

    /// <summary>
    /// Permet le mouvement du vaisseau � l'aide des touches
    /// WASD ou les fl�ches du clavier, seulement possible si le joueur est en vie
    /// </summary>
    private void Bouger()
    {
        if (!_enVie)
        {
            return;
        }
        float dx = Input.GetAxis("Horizontal");
        float dy = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D)) dx = 0;
        else if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow)) dx = 0;
        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.W)) dy = 0;
        else if (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.UpArrow)) dy = 0;

        Vector3 v = new Vector3(dx, dy, 0);

        transform.Translate(v * Time.deltaTime * _vitesse, Space.World);


        //Limite le joueur dans la sc�ne
        float limitex = Mathf.Clamp(transform.position.x, -_limitex, _limitex);
        float limitey = Mathf.Clamp(transform.position.y, -_limitey, _limitey);
        transform.position = new Vector3(limitex, limitey, 0);
    }

    /// <summary>
    /// Permet au joueur de tirer, avec un cooldown selon
    /// la dur�e de l'animation
    /// </summary>
    private void Tirer()
    {
        if (!_enVie)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            if (_peutTirer)
            {

                Quaternion rotationLaser = new Quaternion(0f, 0f, 90f, 90f);
                SoundManager.instance.JouerSon(_sonLaser);
                Instantiate(_prefabLaser, _pointLaser.transform.position, rotationLaser);
                _peutTirer = false;
                _barreRecharge.SetActive(true);
            }
        }
    }

    /// <summary>
    /// Appell� apr�s avoir perdu une vie
    /// </summary>
    private void Mourir()
    {
        EnleverPowerUp();
        SoundManager.instance.JouerSon(_sonExplosion);
        Instantiate(_prefabExplosion, gameObject.transform.position, Quaternion.identity);
        _enVie = false;
        _peutTirer = false;
        _barreRecharge.SetActive(false);
        transform.position = _positionDepart;
        
        //L'animation d<invincibilit� est appel� apr�s l'animation
        _animator.SetTrigger("mourir");
        if (_nbVies == 0)
        {
            GameManager.Instance.GameOver();
        }
    }

    /// <summary>
    /// Uniquement appel� lorsque le powerup
    /// de vitesse est fini
    /// </summary>
    private void EnleverPowerUp()
    {
        _vitesse = _vitesseInitial;
        _flame.SetActive(false);
    }

    // --- M�thodes Publiques -------------------------------------------------------

    /// <summary>
    /// Appel� par L'event d'un animation, qui appelle
    /// par la suite l'animation d'invincibilit�
    /// </summary>
    public void Respawn()
    {
        SoundManager.instance.JouerSon(_sonRespawn, 2f);
        _enVie = true;
        GameManager.Instance.ResumerPointage();
    }

    /// <summary>
    /// Appel� apr�s l'animation d'invincibilit� 
    /// </summary>
    public void EnleveProtection()
    {
        _peutTirer = true;
        _collision.enabled = true;
    }

    /// <summary>
    ///D�sactive un coeur et check si le joeur poss�de encore des vies
    /// </summary>
    public void PerdreVies()
    {
        _collision.enabled = false; //Initialement dans la fonction Mourir, mais je l'ai mis ici pour s'assurer qu'on
                                    //ne pert pas deux vies de suite
        if(_AUnBouclier)
        {
            _bouclier.SetActive(false);
            SoundManager.instance.JouerSon(_sonBouclierBriser);
            _animator.SetTrigger("invincible");
            _AUnBouclier = false;
            _peutTirer = false;
            return;
        }

        _nbVies--;

        for(int i = 0; i < _coeurs.Length; i++) 
        {
            if(i < _nbVies)
            {
                Debug.Log("inchang�");
                _coeurs[i].SetActive(true);
            }
            else
            {
                Debug.Log("perdre un coeur");
                _coeurs[i].SetActive(false);
            }
        }
        GameManager.Instance.ArreterPointage();
        Mourir();
    }

    /// <summary>
    /// Permet au joueur de tirer � nouveau
    /// </summary>
    public void Recharger()
    {
        if (_collision.enabled) _peutTirer = true;
    }

    /// <summary>
    /// Affecte le joueur selon le numero re�u :
    /// 1 : Plus de vitesse
    /// 2 : Bouclier
    /// 3 : Ajoute des points (l'effet est instanti� par le powerup)
    /// </summary>
    /// <param name="numeroPower">Le numero de sprite du powerup</param>
    public void ObtenirPowerup(int numeroPower)
    {
        //Rajoute de la vitesse
        if(numeroPower == 0)
        {
            _vitesse = _vitesseInitial; //Pour s'assurer que les bonus ne stack pas
            _vitesse += _bonusVitesse;
            _flame.SetActive(true);
            CancelInvoke("EnleverPowerUp"); //Au cas o� le joueur est d�j� rapide
            Invoke("EnleverPowerUp", 5f);
            SoundManager.instance.JouerSon(_sonVitesse, 1.5f);
        }
        
        //Donne un Bouclier
        if (numeroPower == 1)
        {
            SoundManager.instance.JouerSon(_sonBouclier);
            if (_AUnBouclier) return;
            _bouclier.SetActive(true);
            _AUnBouclier = true;
        }

        //ajoute des points
        if (numeroPower == 2)
        {
            SoundManager.instance.JouerSon(_sonBonus);
            GameManager.Instance.AjouterPointsBonus(100);
        }
    }
     
}
