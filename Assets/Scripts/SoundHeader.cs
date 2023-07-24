/* ==========================================================================================================
 * 
 *		SoundHeader.cs
 *		Circle-AY.Info / U.G.M.
 *		
 *		音周りの定義ファイル
 *		番号で呼ぶと見通しが悪いので名前を定義する
 *		SoundManagerの再生関数と同時に使われることを想定している
 * 
 *		20150321	とりあえず作成
 *		20220810	RSc100用に更新
 *		20221213	WSc101用に更新
 *		20230620	theroom用に更新
 * 
 * ========================================================================================================== 
 */

using UnityEngine;
using System.Collections;

public class SoundHeader
{

	//曲列挙.
	public enum BGM
	{
		BGM_MAX
	}

	//SE列挙.
	public enum SE
	{
		WILL_ZAP=0,
  JACK,
  OSTRICH1,
  OSTRICH2,
  TONARI_NIKi,
  SE_MAX
	}
		
	//音声列挙.
	public enum VOICE
	{
		Count
	}

}
