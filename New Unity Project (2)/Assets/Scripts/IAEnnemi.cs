
using UnityEngine;
using UnityEngine.AI;

public class IAEnnemi : MonoBehaviour
{
    public NavMeshAgent ennemi;

    public Transform joueur;

    public LayerMask leSol, leJoueur;

    public float vie;

    //Patrolle
    public Vector3 pointMarche;
    bool pointMarcheDonne;
    public float zonePointMarche;

    //Attaque
    public float delaiEntreAttaque;
    bool dejaAttaquer;
    public GameObject projectile;

    //Etat
    public float zoneDetection, zoneAttaque;
    public bool joueurDansZoneDetection, joueurDansZoneAttaque;

    private void Awake()
    {
        joueur = GameObject.Find("PlayerObj").transform;
        ennemi = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //Verifier la vision et la zone d'attaque
        joueurDansZoneDetection = Physics.CheckSphere(transform.position, zoneDetection, leJoueur);
        joueurDansZoneAttaque = Physics.CheckSphere(transform.position, zoneAttaque, leJoueur);

        if (!joueurDansZoneDetection && !joueurDansZoneAttaque) Patrolle();
        if (joueurDansZoneDetection && !joueurDansZoneAttaque) PoursuivreJoueur();
        if (joueurDansZoneAttaque && joueurDansZoneDetection) AttaquerJoueur();
    }

    private void Patrolle()
    {
        if (!pointMarcheDonne) RecherchePointMarche();

        if (pointMarcheDonne)
            ennemi.SetDestination(pointMarche);

        Vector3 distanceAPointMarche = transform.position - pointMarche;

        //Destination atteinte
        if (distanceAPointMarche.magnitude < 1f)
            pointMarcheDonne = false;
    }
    private void RecherchePointMarche()
    {
        //Calcule un point aleatoire dans la zone
        float randomZ = Random.Range(-zonePointMarche, zonePointMarche);
        float randomX = Random.Range(-zonePointMarche, zonePointMarche);

        pointMarche = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(pointMarche, -transform.up, 2f, leSol))
            pointMarcheDonne = true;
    }

    private void PoursuivreJoueur()
    {
        ennemi.SetDestination(joueur.position);
    }

    private void AttaquerJoueur()
    {
        //S'assurer que l'ennemi ne bouge pas
        ennemi.SetDestination(transform.position);

        transform.LookAt(joueur);

        if (!dejaAttaquer)
        {
            ///Code pour attaquer
            Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * 8f, ForceMode.Impulse);
            ///Fin du code d'attaque

            dejaAttaquer = true;
            Invoke(nameof(ReinitAttaque), delaiEntreAttaque);
        }
    }
    private void ReinitAttaque()
    {
        dejaAttaquer = false;
    }

    public void PrendreDegat(int degat)
    {
        vie -= degat;

        if (vie <= 0) Invoke(nameof(DetruireEnnemi), 0.5f);
    }
    private void DetruireEnnemi()
    {
        Destroy(gameObject);
    }

    //Permet de voir les zones d'attaque et de detection
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, zoneAttaque);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, zoneDetection);
    }
}
