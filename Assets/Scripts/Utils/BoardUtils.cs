using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BoardUtils {
    private static List<BaseBoardSpace> tmpSeq = new List<BaseBoardSpace>();

    public static void PlaceItemsRandom(BaseBoardSpace[] board, List<Item> items, Func<BaseBoardSpace, bool> filter) {
        tmpSeq.Clear();
        foreach (BaseBoardSpace space in board) {
            if (filter(space)) tmpSeq.Add(space);
        }

        for (int i = 0; i < items.Count; i++) {
            BaseBoardSpace s = PopSeq();
            items[i].Place(s);
        }
    }

    public static void PlaceItemRandom(BaseBoardSpace[] board, Item item, Func<BaseBoardSpace, bool> filter) {
        tmpSeq.Clear();
        foreach (BaseBoardSpace space in board) {
            if (filter(space)) tmpSeq.Add(space);
        }

        item.Place(FromSeq());
    }

    private static BaseBoardSpace FromSeq() {
        return tmpSeq[UnityEngine.Random.Range(0, tmpSeq.Count)];
    }

    private static BaseBoardSpace PopSeq() {
        int i = UnityEngine.Random.Range(0, tmpSeq.Count);
        BaseBoardSpace ret = tmpSeq[i];
        tmpSeq.RemoveAt(i);
        return ret;
    }
}
