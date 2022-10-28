
using GameDev.Library;

using HmsPlugin;

using HuaweiMobileServices.Game;
using HuaweiMobileServices.Utils;

using UnityEngine;

namespace StackGamePlay
{
    public class ScoreManager : MonoBehaviour
    {

        [SerializeField]
        private string leaderBoardId;

        [SerializeField]
        private string achievementId;

        public string LeaderBoardId { get => leaderBoardId; set => leaderBoardId = value; }
        public string AchievementId { get => achievementId; set => achievementId = value; }



        #region Singleton

        public static ScoreManager Instance;

        private void Singleton()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }

            DontDestroyOnLoad(gameObject);

        }
        #endregion

        #region Monobehaviour
        private void Awake()
        {
            Singleton();
        }
        private void OnEnable()
        {
            GLog.Log($"OnEnable", GLogName.ScoreManager);
            DelegateStore.ShowLeaderboard += OnShowLeaderboard;
            DelegateStore.ShowAchievements += OnShowAchievements;
        }
        private void OnDisable()
        {
            GLog.Log($"OnDisable", GLogName.ScoreManager);
            DelegateStore.ShowLeaderboard -= OnShowLeaderboard;
            DelegateStore.ShowAchievements -= OnShowAchievements;
        }
        void Start()
        {
            StartMethod();
        }
        #endregion


        public void SendScore(int score)
        {
            HMSLeaderboardManager.Instance.SubmitScore(LeaderBoardId, score);
            if (score >= 10)
            {
                HMSAchievementsManager.Instance.UnlockAchievement(AchievementId);
            }
        }

        #region Callbacks
        private void StartMethod()
        {
            HMSGameServiceManager.Instance.OnGetPlayerInfoSuccess = OnGetPlayerInfoSuccess;
            HMSGameServiceManager.Instance.OnGetPlayerInfoFailure = OnGetPlayerInfoFailure;
            HMSAchievementsManager.Instance.OnShowAchievementsSuccess = OnShowAchievementsSuccess;
            HMSAchievementsManager.Instance.OnShowAchievementsFailure = OnShowAchievementsFailure;
            HMSAchievementsManager.Instance.OnRevealAchievementSuccess = OnRevealAchievementSuccess;
            HMSAchievementsManager.Instance.OnRevealAchievementFailure = OnRevealAchievementFailure;
            HMSAchievementsManager.Instance.OnIncreaseStepAchievementSuccess = OnIncreaseStepAchievementSuccess;
            HMSAchievementsManager.Instance.OnIncreaseStepAchievementFailure = OnIncreaseStepAchievementFailure;
            HMSAchievementsManager.Instance.OnUnlockAchievementSuccess = OnUnlockAchievementSuccess;
            HMSAchievementsManager.Instance.OnUnlockAchievementFailure = OnUnlockAchievementFailure;
        }
        private void OnGetPlayerInfoSuccess(Player player)
        {
            Debug.Log("HMS Games: GetPlayerInfo SUCCESS");
        }
        private void OnGetPlayerInfoFailure(HMSException exception)
        {
            Debug.Log("HMS Games: GetPlayerInfo ERROR:" + exception.Message);
        }
        public void ShowArchive()
        {
            HMSSaveGameManager.Instance.ShowArchive();
        }
        public void ShowAchievements()
        {
            HMSAchievementsManager.Instance.ShowAchievements();
        }
        private void OnShowAchievementsSuccess()
        {
            Debug.Log("HMS Games: ShowAchievements SUCCESS ");
        }
        private void OnShowAchievementsFailure(HMSException exception)
        {
            Debug.Log("HMS Games: ShowAchievements ERROR ");
        }
        public void OnRevealAchievementSuccess()
        {
            Debug.Log("HMS Games: RevealAchievement SUCCESS ");
        }
        private void OnRevealAchievementFailure(HMSException error)
        {
            Debug.Log("HMS Games: RevealAchievement ERROR ");
        }
        private void OnIncreaseStepAchievementSuccess()
        {
            Debug.Log("HMS Games: IncreaseStepAchievement SUCCESS ");
        }
        private void OnIncreaseStepAchievementFailure(HMSException error)
        {
            Debug.Log("HMS Games: IncreaseStepAchievement ERROR ");
        }

        private void OnUnlockAchievementSuccess()
        {
            Debug.Log("HMS Games: UnlockAchievement SUCCESS ");
        }
        private void OnUnlockAchievementFailure(HMSException error)
        {
            Debug.Log("HMS Games: UnlockAchievement ERROR ");
        }
        public void GetUserScoreShownOnLeaderboards()
        {
            HMSLeaderboardManager.Instance.IsUserScoreShownOnLeaderboards();
            HMSLeaderboardManager.Instance.OnIsUserScoreShownOnLeaderboardsSuccess = OnIsUserScoreShownOnLeaderboardsSuccess;
            HMSLeaderboardManager.Instance.OnIsUserScoreShownOnLeaderboardsFailure = OnIsUserScoreShownOnLeaderboardsFailure;
        }
        private void OnIsUserScoreShownOnLeaderboardsSuccess(int i)
        {
            Debug.Log("HMS Games: GetUserScoreShownOnLeaderboards SUCCESS ");
        }
        private void OnIsUserScoreShownOnLeaderboardsFailure(HMSException error)
        {
            Debug.Log("HMS Games: GetUserScoreShownOnLeaderboards ERROR ");
        }
        public void SetUserScoreShownOnLeaderboards(int active)
        {
            HMSLeaderboardManager.Instance.SetUserScoreShownOnLeaderboards(active);
            HMSLeaderboardManager.Instance.OnSetUserScoreShownOnLeaderboardsSuccess = OnSetUserScoreShownOnLeaderboardsSuccess;
            HMSLeaderboardManager.Instance.OnSetUserScoreShownOnLeaderboardsFailure = OnSetUserScoreShownOnLeaderboardsFailure;
        }
        private void OnSetUserScoreShownOnLeaderboardsSuccess(int active)
        {
            Debug.Log("HMS Games: SetUserScoreShownOnLeaderboards SUCCESS ");
        }
        private void OnSetUserScoreShownOnLeaderboardsFailure(HMSException exception)
        {
            Debug.Log("HMS Games: SetUserScoreShownOnLeaderboards ERROR ");
        }
        public void SubmitScore(string leaderboardId, long score, string scoreTips)
        {
            HMSLeaderboardManager.Instance.OnSubmitScoreSuccess = OnSubmitScoreSuccess;
            HMSLeaderboardManager.Instance.OnSubmitScoreFailure = OnSubmitScoreFailure;
            //if (customUnit)
            //{
            //    HMSLeaderboardManager.Instance.SubmitScore(leaderboardId, score, scoreTips);
            //}
            //else
            //{
            //    HMSLeaderboardManager.Instance.SubmitScore(leaderboardId, score);
            //}
        }
        private void OnSubmitScoreSuccess(ScoreSubmissionInfo scoreSubmission)
        {
            Debug.Log("HMS Games: SubmitScore SUCCESS ");
        }
        private void OnSubmitScoreFailure(HMSException exception)
        {
            Debug.Log("HMS Games: SubmitScore ERROR ");
        }
        public void ShowLeaderboards()
        {
            HMSLeaderboardManager.Instance.ShowLeaderboards();
            HMSLeaderboardManager.Instance.OnShowLeaderboardsSuccess = OnShowLeaderboardsSuccess;
            HMSLeaderboardManager.Instance.OnShowLeaderboardsFailure = OnShowLeaderboardsFailure;
        }
        private void OnShowLeaderboardsSuccess()
        {
            Debug.Log("HMS Games: ShowLeaderboards SUCCESS ");
        }
        private void OnShowLeaderboardsFailure(HMSException error)
        {
            Debug.Log("HMS Games: ShowLeaderboards ERROR ");
        }
        public void GetLeaderboardsData(string leaderboardId)
        {
            HMSLeaderboardManager.Instance.GetLeaderboardData(leaderboardId);
            HMSLeaderboardManager.Instance.OnGetLeaderboardDataSuccess = OnGetLeaderboardDataSuccess;
            HMSLeaderboardManager.Instance.OnGetLeaderboardsDataFailure = OnGetLeaderboardsDataFailure;
        }
        private void OnGetLeaderboardDataSuccess(Ranking ranking)
        {
            Debug.Log("HMS Games: GetLeaderboardsData SUCCESS ");
        }
        private void OnGetLeaderboardsDataFailure(HMSException exception)
        {
            Debug.Log("HMS Games: GetLeaderboardsData ERROR ");
        }
        public void GetScoresFromLeaderboard(string leaderboardId, int timeDimension, int maxResults, int offsetPlayerRank, int pageDirection)
        {
            HMSLeaderboardManager.Instance.GetScoresFromLeaderboard(leaderboardId, timeDimension, maxResults, offsetPlayerRank, pageDirection);
            HMSLeaderboardManager.Instance.OnGetScoresFromLeaderboardSuccess = OnGetScoresFromLeaderboardSuccess;
            HMSLeaderboardManager.Instance.OnGetScoresFromLeaderboardFailure = OnGetScoresFromLeaderboardFailure;
        }
        private void OnGetScoresFromLeaderboardSuccess(RankingScores rankingScores)
        {
            Debug.Log("HMS Games: GetScoresFromLeaderboard SUCCESS ");
        }
        private void OnGetScoresFromLeaderboardFailure(HMSException error)
        {
            Debug.Log("HMS Games: GetScoresFromLeaderboard ERROR ");
        }
        #endregion


        #region Event: ShowLeaderboard
        private void OnShowLeaderboard()
        {
            Debug.Log("OnShowLeaderboard");
            HMSLeaderboardManager.Instance.ShowLeaderboards();
        }
        #endregion

        #region Event: ShowAchievements
        private void OnShowAchievements()
        {
            Debug.Log("OnShowAchievements");
            HMSAchievementsManager.Instance.ShowAchievements();
        }
    }
    #endregion

}



