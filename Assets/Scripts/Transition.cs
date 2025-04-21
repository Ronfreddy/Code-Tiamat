using DG.Tweening;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour
{
    public Transform[] cells;
    public int[] randomint;


    private void Start()
    {
        randomint = GenerateShuffledArray(cells.Length);
    }

    private int[] GenerateShuffledArray(int size)
    {
        List<int> list = new List<int>();

        for (int i = 0; i < size; i++)
        {
            list.Add(i);
        }

        // Shuffle using Fisher-Yates
        for (int i = list.Count - 1; i > 0; i--)
        {
            int rand = Random.Range(0, i + 1);
            int temp = list[i];
            list[i] = list[rand];
            list[rand] = temp;
        }

        return list.ToArray();
    }

    public void FadeIn()
    {
        Sequence sequence = DOTween.Sequence();
        foreach (int i in randomint)
        {
            sequence.Append(cells[i].DOLocalRotate(new Vector3(90, 0, 0), 0.007f));
        }
        sequence.SetUpdate(true);
        sequence.Play();
    }

    public void FadeOut() 
    {
        Sequence sequence = DOTween.Sequence();
        foreach (int i in randomint)
        {
            sequence.Append(cells[i].DOLocalRotate(new Vector3(0, 0, 0), 0.007f));
        }
        sequence.Play();
    }
}
