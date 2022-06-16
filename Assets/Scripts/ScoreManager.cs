using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    private static float _score = 0.0f;
    private static float _combo = 0.0f;
    private static float _tolerance = 2.5f;
    private static float _timeToleranceAmount = 10.0f;
    private static float _penaltyFactorTime = 2.0f;
    private static float _penaltyFactorCutAngle = 2.0f;
    private static float _penaltyFactorOffsetAngle = 1.25f;
    private static float _maxHealth = 100.0f;
    private static float _currentHealth = 100.0f;

    private IEnumerator _hurtCoroutine;

    [SerializeField]
    private TextMeshPro _textMeshProScore;

    [SerializeField]
    private TextMeshPro _textMeshProCombo;

    [SerializeField]
    private Image _redSplatter;

    public static ScoreManager Instance { get; private set; }

    private IEnumerator HurtCoroutine()
    {
        Debug.Log("HELLO");
        yield return new WaitForSeconds(2.0f);
        while (_currentHealth < _maxHealth)
        {
            Debug.Log(_currentHealth);
            _currentHealth++;
            ModifySplatter();
            yield return new WaitForSeconds(0.05f);
        }
        yield return null;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            _hurtCoroutine = HurtCoroutine();
        }
    }

    public float CalculateScoreBasedOnTime(float cutTime, float goodCutTime)
    {
        float score = 0.0f; 
        float tolerance = _timeToleranceAmount * (_tolerance / 100.0f);

        if (cutTime + tolerance >= goodCutTime && cutTime - tolerance <= goodCutTime)
        {
            score += 100.0f;
        }
        else
        {
            float percentage = cutTime * 100.0f / goodCutTime;
            percentage = 100.0f - Mathf.Abs(percentage - 100.0f) * _penaltyFactorTime;
            score += percentage;
        }

        Mathf.Max(0, score);

        return score;
    }

    public float CalculateScoreBasedOnCutAngle(float angle)
    {
        float score = 0.0f;

        float percentage = angle * 100.0f / 90.0f;
        percentage = 100.0f - percentage * _penaltyFactorCutAngle;
        score += percentage;

        Mathf.Max(0, score);

        return score;
    }

    public float CalculateScoreBasedOnOffsetAngle(float angle)
    {
        float score = 0.0f;

        float percentage = angle * 100.0f / 90.0f;
        percentage = 100.0f - percentage * _penaltyFactorOffsetAngle;
        score += percentage;

        Mathf.Max(0, score);

        return score;
    }
    
    public void IncrementAndUpdateScore(float amount, bool timeBased = true, bool angleBased = true, bool offsetBased = true)
    {
        if (timeBased && angleBased && offsetBased)
            amount /= 3.0f;

        if (_combo > 0)
            amount *= 5.0f * _combo / 100.0f;
        _score += amount;

        _combo++;
        UpdateScore();
    }

    private void UpdateScore()
    {
        _textMeshProScore.text = "SCORE\n" + (int)_score;
        _textMeshProCombo.text = "COMBO\n" + (int)_combo;
    }

    private void ModifySplatter()
    {
        Color splatterAlpha = _redSplatter.color;
        splatterAlpha.a = 1 - (_currentHealth / _maxHealth);
        _redSplatter.color = splatterAlpha;
    }

    private void UpdateCoroutines()
    {
        StopCoroutine(_hurtCoroutine);
        _hurtCoroutine = HurtCoroutine();
        StartCoroutine(_hurtCoroutine);
    }

    public static void TakeDamage(float amount)
    {
        _currentHealth -= amount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0.0f, 100.0f);
        _combo = 0;
        Instance.ModifySplatter();
        Instance.UpdateCoroutines();
        Instance.UpdateScore();
    }

    public static float GetScore()
    {
        return _score;
    }
}
