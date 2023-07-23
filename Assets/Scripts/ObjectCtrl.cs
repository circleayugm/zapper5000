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
	public string TARGET;
	[SerializeField]
	public SphereCollider MainHit2;
	[SerializeField]
	public string TARGET2;
	[SerializeField]
	public CapsuleCollider MainHit3;    // ここだけ隣ニキの通電範囲
	[SerializeField]
	public string TARGET_NOISE;			// 範囲に入ると音が鳴る

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

	public object Find { get; private set; }

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
				MainPic.enabled = false;
				MainHit.enabled = false;
				MainHit2.enabled = false;
				count = 0;
				break;
			case ObjectManager.MODE.HIT:
				MainHit.enabled = true;
				MainHit2.enabled = true; 
				break;
			case ObjectManager.MODE.NOHIT:
				MainHit.enabled = false;
				MainHit2.enabled = false; 
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
					
					MainHit.enabled = true;
					MainHit2.enabled = true;
					MainHit3.enabled = true;
					MainHit.GetComponent<HitName>().PARTS = "hand_just";
					MainHit2.GetComponent<HitName>().PARTS = "hand_good";
					MainHit3.GetComponent<HitName>().PARTS = "hand_noise";

					MainPic.enabled = true;


				}
				param[0] = 0;
				Vector3 inp = new Vector3(0, 0, 0);
				pos = this.transform.localPosition;
				inp.x = Input.GetAxis("Horizontal");
				inp.y = Input.GetAxis("Vertical");
				if (Mathf.Abs(pos.x + inp.x) > 6.0f)
				{
					inp.x = 0.0f;
				}
				if (Mathf.Abs(pos.y + inp.y) >2.0f)
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
					this.gameObject.GetComponent<HitName>().PARTS = "will_body";

					MainHit.enabled = true;
					MainHit2.enabled = true;
					MainHit.GetComponent<HitName>().PARTS = "will_tkb_left";
					MainHit2.GetComponent<HitName>().PARTS = "will_tkb_right";

					param[0] = 0;

					NOHIT = false;
					MainPic.color = COLOR_NORMAL;
					MainPic.sortingOrder = 1;
					power = 1;
					LIFE = 1;
				}
				pos = new Vector3(0, 0, 0.5f);
				if (pos.z > 0)
				{
					pos.z = 0.15f;
				}
				this.transform.localPosition -= pos;

				if (param[0] > 0)
				{
					if (((count >> 1) % 2) == 0)
					{
						MainPic.enabled = true;
					}
					else
					{
						MainPic.enabled = false;
					}
				}
				
				if (this.transform.localPosition.z<-9.0f)
				{
					MANAGE.Return(this);
				}
				break;




			/*
			


			*/
			case ObjectManager.TYPE.TONARI_NIKI:
				if (count == 0)
				{
					obj_mode = ObjectManager.MODE.HIT;
					MainPic.sprite = MANAGE.SPR_ENEMY[1];
					MainPic.enabled = true;
					MainHit.enabled = false;
					MainHit2.enabled = false;
					this.gameObject.GetComponent<HitName>().PARTS = "tonari_niki";
					MainHit.radius = 0.4f;
					NOHIT = false;
					MainPic.color = COLOR_NORMAL;
					MainPic.sortingOrder = 1;
					power = 1;
					LIFE = 1;
					param[0] = 0;
				}
				pos = new Vector3(0, 0, 0.5f);
				if (pos.z > 0)
				{
					pos.z = 0.15f;
				}
				this.transform.localPosition -= pos;

				if (param[0] > 0)
				{
					if (((count >> 1) % 2) == 0)
					{
						MainPic.enabled = true;
					}
					else
					{
						MainPic.enabled = false;
					}
				}

				if (this.transform.localPosition.z < -9.0f)
				{
					MANAGE.Return(this);
				}
				break;





			/*
			


			*/
			case ObjectManager.TYPE.JACK:
				if (count == 0)
				{
					obj_mode = ObjectManager.MODE.HIT;
					MainPic.sprite = MANAGE.SPR_ENEMY[2];
					MainPic.enabled = true;
					MainHit.enabled = false;
					MainHit2.enabled = false;
					this.gameObject.GetComponent<HitName>().PARTS = "jack";
					MainHit.enabled = true;
					MainHit.radius = 0.4f;
					NOHIT = false;
					MainPic.color = COLOR_NORMAL;
					MainPic.sortingOrder = 1;
					power = 1;
					LIFE = 1;
					param[0] = 0;
				}
				pos = new Vector3(0, 0, 0.5f);
				if (pos.z > 0)
				{
					pos.z = 0.15f;
				}

				if (param[0] > 0)
				{
					if (((count >> 1) % 2) == 0)
					{
						MainPic.enabled = true;
					}
					else
					{
						MainPic.enabled = false;
					}
				}

				this.transform.localPosition -= pos;
				if (this.transform.localPosition.z < -9.0f)
				{
					MANAGE.Return(this);
				}
				break;



			/******************************************************** 
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


	private void OnTriggerEnter(Collider other)
	{
		if (obj_mode == ObjectManager.MODE.NOHIT) return;
		ObjectCtrl other_ctrl = other.gameObject.GetComponent<ObjectCtrl>();
		string other_name = other.gameObject.GetComponent<HitName>().PARTS;
		ObjectCtrl mine = this.gameObject.GetComponent<ObjectCtrl>();
		string mine_name = this.gameObject.GetComponent<HitName>().PARTS;
		if(mine_name==null)
		{
			mine_name = "null";
		}
		Debug.Log("mine_name=[" + mine_name + "] other_name=[" + other_name + "]");
		switch(mine_name)
		{
			case "null":
				return;
			case "will_tkb_left":
			case "will_tkb_right":
				if (other_name == "tonari_niki")
				{
					return;
				}
				if (other_name == "jack")
				{
					return;
				}
				if (other_name == "will")
				{
					return;
				}
				if (other_ctrl.param[0]>0)
				{
					return;
				}
				
				//if (other_name=="hand_just")
				if (mine.param[0]>0)
				{
					SoundManager.Instance.PlaySE(0);
					MAIN.player_sats -=100;
				}
				//if (other_name=="hand_good")
				{
					MAIN.player_sats -= 50;
				}
				if (other_name=="hand_noise")
				{
					SoundManager.Instance.PlaySE(4);
				}
				mine.param[0]++;

				break;
			case "tonari_niki":
				if (mine.param[0] > 0)
				{
					mine.param[0]++;
					SoundManager.Instance.PlaySE(4);
				}
				break;
			case "jack":
				//if(other_name=="hand_good")
				if (mine.param[0] > 0)
				{

					SoundManager.Instance.PlaySE(Random.Range(1, 3));
					MAIN.player_sats += 1137;
					mine.param[0]++;

				}
				break;
			case "will_body":
				if (mine.param[0]>0)
				{
					mine.param[0]++;
					MAIN.player_sats -= 100;
				}
				break;
#if false

			case "hand_just":
				switch(other_name)
				{
					case "null":
						return;
					case "will_tkb_left":
					case "will_tkb_right":
						{
							SoundManager.Instance.PlaySE(0);
							MAIN.player_sats -= 500;
						}
						break;
					case "tonari_niki":
						break;
				}
				break;
			case "hand_noise":
				if (other_name == "will")
				{
					MAIN.player_sats -= 300;
				}
				else if (other_name== "jack")
				{
					SoundManager.Instance.PlaySE(Random.Range(1, 3));
					MAIN.player_sats += 1137;
				}
				{
					SoundManager.Instance.PlaySE(4);
				}
				break;
			case "hand_good":
				break;
#endif
		}
	}





#if false

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
#endif

#if false
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
