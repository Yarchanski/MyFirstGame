using System.Collections; // ���������� ������������ ���� ��� ������ � �����������, ��������, ��� IEnumerator.
using System.Collections.Generic; // ���������� ������������ ���� ��� ������ � ����������� ���� List.
using UnityEngine; // ���������� ������������ ���� Unity ��� �������� �����������, ������ ��� Instantiate � MonoBehaviour.

public class SpawnEnemy : MonoBehaviour // ��������� �����, ������� ����� �������� �� �������� ������.
{
    [SerializeField] // ������� ��� ����������� ���������� � ��������� Unity.
    private GameObject[] spawnEnemy; // ������ �������� ������, ������� ����� ���� ����������.

    [SerializeField] // ������� ��� ����������� ���������� � ��������� Unity.
    private Transform[] spawnPoint; // ������ ����� ��� ������ ������.

    public float startSpawnerInterval; // �������� ����� ����������� ������ � �������� (�������� �� ��������� ����������� � ���������).
    private float spawnerInterval; // �����, ������� ������ � ���������� ������, ������������ ��� ������������ �������.

    public int numberOfEnemies; // ����� ���������� ������, ������� ������ ���� ����������.
    private int nowTheEnemies = 0; // ������� ���������� ������������ ������.
    private int randEnemy; // ��������� ������ ��� ������ ���� �����.
    private int randPoint; // ��������� ������ ��� ������ ����� ������.

    // Start is called before the first frame update
    void Start()
    {
        // �������� ��������� �� PlayerPrefs (���� ��� ����) ��� ������������� �������� �� ���������.
        numberOfEnemies = PlayerPrefs.GetInt("NumberOfEnemies", 4); // 4 - �������� �� ��������� ��� ���������� ������.
        startSpawnerInterval = PlayerPrefs.GetFloat("StartSpawnerInterval", 2f); // 2f - �������� �� ��������� ��� ��������� ����� ��������.
    }

    // Update is called once per frame
    void Update()
    {
        // ���� �������� ��� ������ �����, � ���������� ������ ������ �������������.
        if (spawnerInterval <= 0 && nowTheEnemies < numberOfEnemies)
        {
            randEnemy = Random.Range(0, spawnEnemy.Length); // ���������� ��������� ������ ��� �����.
            randPoint = Random.Range(0, spawnPoint.Length); // ���������� ��������� ������ ��� ����� ������.

            // ������� ������ ����� �� ��������� �����.
            GameObject enemy = Instantiate(spawnEnemy[randEnemy], spawnPoint[randPoint].transform.position, Quaternion.identity);

            // ���������, ����� �� ������ ��������� Enemy, � ���� ��, ������������� �� ������� ����������� �����.
            if (enemy.TryGetComponent<Enemy>(out var enemyComponent))
            {
                enemyComponent.OnEnemyDestroyed += DecreaseEnemyCount; // ��� ����������� ����� ���������� ������� ������������ ������.
            }

            spawnerInterval = startSpawnerInterval; // ���������� �������� ��� ���������� ������.
            nowTheEnemies++; // ����������� ���������� ������������ ������.
        }
        else
        {
            // ���� �������� ��� �� �����, ��������� �����.
            spawnerInterval -= Time.deltaTime;
        }
    }

    private void DecreaseEnemyCount()
    {
        // �����, ������� ����������� ���������� ������������ ������, ����� ���� �� ��� ������������.
        nowTheEnemies--;
    }
}
