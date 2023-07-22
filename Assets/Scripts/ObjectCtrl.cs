/*
 * 
 *	ObjectCtrl.cs
 *		オブジェクトの固有動作の管理
 * 
 * 
 * 
 * 
 * 
 *		20221211	WSc101用に再構成
 * 
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectCtrl : MonoBehaviour
{
	[Space]
	[SerializeField]
	public int OBJcnt = 0;
	[Space]
	[SerializeField]
	public SpriteRenderer MainPic;		// メイン画像.(プライオリティ前面)
	[SerializeField]
	public Transform MainPos;           // 座標・回転関連.
	[SerializeField]
	public SphereCollider MainHit;         // 当たり判定.
	[SerializeField]
	public SphereCollider MainHit2;
	
	[Space]

	public int LIFE = 0;		// 耐久力.
	public bool NOHIT = false;	// 当たり判定の有無.
	[Space]
	public int speed = 0;						// 移動速度.
	public int angle = 0;						// 移動角度(360度を256段階で指定).
	public int oldangle = 0;					// 1int前の角度
	public int type = 0;						// キャラクタタイプ(同じキャラクタだけど動きが違うなどの振り分け).
	public int mode = 0;						// 動作モード(キャラクタによって意味が違う).
	public int power = 0;						// 相手に与えるダメージ量.
	public int count = 0;						// 動作カウンタ.
	public int[] param = new int[4];			// パラメータ4個
	public Vector3[] parampos = new Vector3[4];	// テンポラリ座標4個
	public Vector3 vect = Vector3.zero;			// 移動量
	public int interval = 0;					// 0で自爆しない・任意の数値を入れるとカウント到達時に自爆




	[Space]

	readonly Color COLOR_NORMAL = new Color(1.0f, 1.0f, 1.0f, 1.0f);
	readonly Color COLOR_DAMAGE = new Color(1.0f, 0.0f, 0.0f, 1.0f);
	readonly Color COLOR_ERASE = new Color(0.0f, 0.0f, 0.0f, 0.0f);

	const float SHIP_MOVE_SPEED = 0.10f;
	const float WATER_MOVE_SPEED = 0.54f;

	const float WATER_PERCENTAGE = 0.001f;

	const float OFFSCREEN_MIN_X = -6.00f;
	const float OFFSCREEN_MAX_X = 6.00f;
	const float OFFSCREEN_MIN_Y = -4.00f;
	const float OFFSCREEN_MAX_Y = 4.00f;

	const float HITSIZE_MYSHIP = 0.16f;
	const float HITSIZE_MYSHOT = 0.64f;
	const float HITSIZE_ENEMY = 0.32f;

	public Vector3 myinp;

	public ObjectManager.MODE obj_mode = ObjectManager.MODE.NOUSE;  // キャラクタの管理状態.
	public ObjectManager.TYPE obj_type = ObjectManager.TYPE.NOUSE;  // キャラクタの分類(当たり判定時に必要).

	MainSceneCtrl MAIN;
	ObjectManager MANAGE;

	void Awake()
	{
		MAIN = GameObject.Find("root_game").GetComponent<MainSceneCtrl>();
		MANAGE = GameObject.Find("root_game").GetComponent<ObjectManager>();
	}


	// Use this for initialization
	void Start()
	{
		for (int i = 0; i < param.Length; i++)
		{
			param[i] = 0;
		}
	}
	// Update is called once per frame
	void Update()
	{
		Vector3 pos = Vector3.zero;

		if (obj_mode == ObjectManager.MODE.NOUSE)
		{
			return;
		}
#if false
		if (ModeMANAGEr.mode == ModeMANAGEr.MODE.GAME_PAUSE)
		{
			return;
		}
#endif

		switch (obj_mode)
		{
			case ObjectManager.MODE.NOUSE:
				return;
			case ObjectManager.MODE.INIT:
				MainPic.enabled = true;
				count = 0;
				break;
			case ObjectManager.MODE.HIT:
				MainHit.enabled = true;
				break;
			case ObjectManager.MODE.NOHIT:
				MainHit.enabled = false;
				break;
			case ObjectManager.MODE.FINISH:
				MANAGE.Return(this);
				break;
		}
		switch (obj_type)
		{
			case ObjectManager.TYPE.TONARI_NIKI_HAND:
				if (count == 0)
				{
					this.transform.localPosition = new Vector3(0, 0, 0);

				}
				Vector3 inp = new Vector3(0, 0, 0);
				pos = this.transform.localPosition;
				inp.x = Input.GetAxis("Horizontal");
				inp.y = Input.GetAxis("Vertical");
				if (Mathf.Abs(pos.x + inp.x) > 16.0f)
				{
					inp.x = 0.0f;
				}
				if (Mathf.Abs(pos.y + inp.y) > 8.0f)
				{
					inp.y = 0.0f;
				}
				this.transform.localPosition += inp;


				break;

			/*
			


			*/
			case ObjectManager.TYPE.WILL:
				if (count == 0)
				{
					obj_mode = ObjectManager.MODE.HIT;
					MainPic.sprite = MANAGE.SPR_ENEMY[0];
					MainPic.enabled = true;
					MainHit.enabled = true;
					MainHit2.enabled = true;
					
					NOHIT = false;
					MainPic.color = COLOR_NORMAL;
					MainPic.sortingOrder = 1;
					power = 1;
					LIFE = 1;
				}
				pos = new Vector3(0, 0, 0.5f);
				if (pos.z > -3)
				{
					pos.z = 0.13f;
				}
				this.transform.localPosition -= pos;
				if (this.transform.localPosition.z<-9.0f)
				{
					MANAGE.Return(this);
				}
				break;




#if false
/*
			壁



			*/
			case ObjectManager.TYPE.TONARI_NIKI:
				if (count == 0)
				{
					switch (mode)
					{
						case 0:
							obj_mode = ObjectManager.MODE.HIT;
							MainPic.sprite = MANAGE.SPR_WALL[0];
							MainHit.enabled = true;
							
							NOHIT = false;
							break;
						case 1:
							obj_mode = ObjectManager.MODE.NOHIT;
							MainPic.sprite = MANAGE.SPR_WALL[1];
							MainHit.enabled = false;
							NOHIT = true;
							break;
					}
					MainPic.color = COLOR_NORMAL;
					MainPic.sortingOrder = -1;
					power = 1;
					LIFE = 1;
				}
				break;
#endif



			/******************************************************
			 * 
			 * 
			 * 
			 * 
			 * ここからエフェクトなど
			 * 
			 * 
			 *
			 ******************************************************
			 */
			case ObjectManager.TYPE.NOHIT_EFFECT:
				if (count == 0)
				{
					obj_mode = ObjectManager.MODE.NOHIT;
					MainHit.enabled = false;
					MainPic.enabled = true;
					LIFE = 1;
					vect = MANAGE.AngleToVector3(angle, speed * 0.05f);
					MainPic.sprite = MANAGE.SPR_CRUSH[0];
					MainPic.sortingOrder = 5;
				}
				else if (count >= 16)
				{
					MANAGE.Return(this);
				}
				else
				{
					this.transform.localPosition += vect;
					this.transform.localScale = this.transform.localScale * 1.1f;
					MainPic.sprite = MANAGE.SPR_CRUSH[count >> 1];
				}
				break;
		}

		// 自前衝突判定を使う場合
		//MANAGE.CheckHit(this);

		if (LIFE <= 0)	// 死亡確認
		{
			Dead();
		}

		count++;
	}




	/// <summary>
	/// 当たり判定部・スプライト同士が衝突した時に走る
	/// </summary>
	/// <param name="collider">衝突したスプライト当たり情報</param>
	void OnTriggerStay2D(Collider2D collider)
	{
		if (obj_mode == ObjectManager.MODE.NOHIT)
		{
			return;
		}
		ObjectCtrl other = collider.gameObject.GetComponent<ObjectCtrl>();
		if (other.obj_mode == ObjectManager.MODE.NOHIT)
		{
			return;
		}
		if (NOHIT == true)
		{
			return;
		}
		if (other.NOHIT == true)
		{
			return;
		}
		switch (other.obj_type)
		{
			case ObjectManager.TYPE.TONARI_NIKI_HAND:
				{
					if (obj_type == ObjectManager.TYPE.TONARI_NIKI)
					{
						if (other.obj_mode == ObjectManager.MODE.HIT)
						{
							param[0] = 1;
							Vector3 newpos = this.transform.localPosition;
							if (
									(this.transform.localPosition.x < this.parampos[0].x)
								||	(this.transform.localPosition.x > this.parampos[0].x)
								)
							{
								this.vect.x = 0.0f;
								newpos.x = this.parampos[0].x;
								newpos.y = this.transform.localPosition.y;
							}
							if (
									(this.transform.localPosition.y < this.parampos[0].y)
								||  (this.transform.localPosition.y > this.parampos[0].y)
								)
							{
								this.vect.y = 0.0f;
								newpos.x = this.transform.localPosition.x;
								newpos.y = this.parampos[0].y;
							}
							this.transform.localPosition = newpos;
						}
						Debug.Log("wall check:oldpos=" + this.parampos[0] + " / newpos=" + this.transform.localPosition + " / vect=" + other.vect);
					}
				}
				break;
		}
	}


#if true
	/// <summary>
	/// 当たり判定部・スプライト同士が衝突した時に走る
	/// </summary>
	/// <param name="collider">衝突したスプライト当たり情報</param>
	void OnTriggerEnter2D(Collider2D collider)
	{
		if (obj_mode == ObjectManager.MODE.NOHIT)
		{
			return;
		}
		ObjectCtrl other = collider.gameObject.GetComponent<ObjectCtrl>();
		if (other.obj_mode == ObjectManager.MODE.NOHIT)
		{
			return;
		}
		if (NOHIT == true)
		{
			return;
		}
		if (other.NOHIT == true)
		{
			return;
		}
		switch (other.obj_type)
		{
			case ObjectManager.TYPE.TONARI_NIKI_HAND:
				{
					if (obj_type == ObjectManager.TYPE.TONARI_NIKI)
					{
						if (other.obj_mode == ObjectManager.MODE.HIT)
						{
							this.param[0] = 1;
							Vector3 newpos = this.transform.localPosition;
							if (
									(this.transform.localPosition.x < this.parampos[0].x)
								|| (this.transform.localPosition.x > this.parampos[0].x)
								)
							{
								this.vect.x = 0.0f;
								newpos.x = this.parampos[0].x;
								newpos.y = this.transform.localPosition.y;
							}
							if (
									(this.transform.localPosition.y < this.parampos[0].y)
								|| (this.transform.localPosition.y > this.parampos[0].y)
								)
							{
								this.vect.y = 0.0f;
								newpos.x = this.transform.localPosition.x;
								newpos.y = this.parampos[0].y;
							}
							this.transform.localPosition = newpos;
						}
						Debug.Log("wall check:oldpos=" + parampos[0] + " / newpos=" + transform.localPosition + " / vect=" + other.vect);
					}
				}
				break;
		}
	}
#endif

	/// <summary>
	/// ダメージ与える
	/// </summary>
	/// <param name="damage">ダメージ量</param>
	public void Damage(int damage)
	{
		LIFE -= damage;
			if (LIFE <= 0)
			{
				//Dead();	// リプレイある時はダメージ関数で死亡処理を行わない
			}
	}

	/// <summary>
	///		死んだ時の処理全般
	/// </summary>
	public void Dead()
	{
		obj_mode = ObjectManager.MODE.NOHIT;
		switch (obj_type)
		{
			default:
				break;
		}
		MainPic.color = COLOR_NORMAL;
		count = 0;
	}

	public void DisplayOff()
	{
		MainPic.enabled = false;
		MainHit.enabled = false;
		MainHit2.enabled = false;
		MainPic.color = COLOR_NORMAL;
		MainPic.sprite = null;
	}

}
