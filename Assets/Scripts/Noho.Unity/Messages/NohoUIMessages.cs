using Noho.Managers;
using Noho.Messages;
using Noho.Unity.Models;
using Noho.Unity.Scenes.Surgery.Gunk;
using UnityEngine;

namespace Noho.Unity.Messages
{
    public class DefibrillatorActiveMsg : BaseNohoMessage
    {
        public override MessageConstants.NohoMsgType NohoMsgType => MessageConstants.NohoMsgType.DEFIBRILLATOR_ACTIVE;
    }
    public class SutureFinishedMessage : BaseNohoMessage
    {
        public override MessageConstants.NohoMsgType NohoMsgType => MessageConstants.NohoMsgType.SUTURE_FINISH;
    }

    public class ClearedSmallCutMsg : BaseNohoMessage
    {
        public SmallCutController SmallCut;
        public override MessageConstants.NohoMsgType NohoMsgType => MessageConstants.NohoMsgType.CLEARED_SMALL_CUT;
    }

    public class ShowWinMsg : BaseNohoMessage
    {
        public Win Win;
        public override MessageConstants.NohoMsgType NohoMsgType => MessageConstants.NohoMsgType.CLEARED_SMALL_CUT;
    }

    public class ClearSuturesMsg : BaseNohoMessage
    {
        public override MessageConstants.NohoMsgType NohoMsgType => MessageConstants.NohoMsgType.CLEAR_SUTURES;
    }

    public class DialogueDisplayedMsg : BaseNohoMessage
    {
        public string Speaker;
        public string Dialogue;
        public Color Color;
        public override MessageConstants.NohoMsgType NohoMsgType => MessageConstants.NohoMsgType.DIALOGUE_DISPLAYED;
    }

    public class SmallCutUnobstructedMsg : BaseNohoMessage
    {
        public SmallCutController SmallCut;
        public override MessageConstants.NohoMsgType NohoMsgType => MessageConstants.NohoMsgType.SMALL_CUT_UNOBSTRUCTED;
    }

    public class SmallCutObstructedMsg : BaseNohoMessage
    {
        public SmallCutController SmallCut;
        public override MessageConstants.NohoMsgType NohoMsgType => MessageConstants.NohoMsgType.SMALL_CUT_OBSTRUCTED;
    }

    public class CutSpotMsg : BaseNohoMessage
    {
        public IncisionSpotController CutSpot;
        
        public override MessageConstants.NohoMsgType NohoMsgType => MessageConstants.NohoMsgType.CUT_SPOT;
    }

    public class SpotGelledMsg : BaseNohoMessage
    {
        public IncisionSpotController CutSpot;
        
        public override MessageConstants.NohoMsgType NohoMsgType => MessageConstants.NohoMsgType.SPOT_GELLED;
    }

    public class ChoiceSelectedMsg : BaseNohoMessage
    {
        public int ChoiceIdx;
        public StageActionManager SAM;
        public override MessageConstants.NohoMsgType NohoMsgType => MessageConstants.NohoMsgType.CHOICE_SELECTED;
    }

    public class FragmentClearedMsg : BaseNohoMessage
    {
        public GameObject Fragment;
        public override MessageConstants.NohoMsgType NohoMsgType => MessageConstants.NohoMsgType.FRAGMENT_CLEARED;
    }

    public class FragmentPlacedMsg : BaseNohoMessage
    {
        public GameObject Fragment;
        public override MessageConstants.NohoMsgType NohoMsgType => MessageConstants.NohoMsgType.FRAGMENT_CLEARED;
    }

    public class GunkAppearedMsg : BaseNohoMessage
    {
        public MonoBehaviour Gunk;
        public override MessageConstants.NohoMsgType NohoMsgType => MessageConstants.NohoMsgType.FRAGMENT_CLEARED;
    }

    public class StartNewProcedureStepMsg : BaseNohoMessage
    {
        public string Tag;
        public ProcedureStep Step;
        public override MessageConstants.NohoMsgType NohoMsgType => MessageConstants.NohoMsgType.START_NEW_PROCEDURE_STEP;
    }

    public class EarnScoreMsg : BaseNohoMessage
    {
        public int ScoreDelta;
        public override MessageConstants.NohoMsgType NohoMsgType => MessageConstants.NohoMsgType.EARN_SCORE;
    }

    public class GunkTotallyClearedMsg : BaseNohoMessage
    {
        public string GunkTag;
        public string NextGunkTag;
        public override MessageConstants.NohoMsgType NohoMsgType => MessageConstants.NohoMsgType.GUNK_TOTALLY_CLEARED;
    }

    public class CloseWinMsg : BaseNohoMessage
    {
        public override MessageConstants.NohoMsgType NohoMsgType => MessageConstants.NohoMsgType.CLOSE_WIN;
    }

    public class SAMCompletedMsg : BaseNohoMessage
    {
        public override MessageConstants.NohoMsgType NohoMsgType => MessageConstants.NohoMsgType.SAM_COMPLETED;
    }

    public class CharacterLoveChangedMsg : BaseNohoMessage
    {
        public string CharacterDevName;
        public int LoveDelta;
        public override MessageConstants.NohoMsgType NohoMsgType => MessageConstants.NohoMsgType.CHARACTER_LOVE_CHANGED;
    }

    public class InjectedMsg : BaseNohoMessage
    {
        public override MessageConstants.NohoMsgType NohoMsgType => MessageConstants.NohoMsgType.INJECTED;
    }

    public class NewNotifMsg : BaseNohoMessage
    {
        public string Notif;
        public override MessageConstants.NohoMsgType NohoMsgType => MessageConstants.NohoMsgType.SHOW_NOTIF;
    }

    public class PlayAudioMsg : BaseNohoMessage
    {
        public AudioClip AudioClip;
        public override MessageConstants.NohoMsgType NohoMsgType => MessageConstants.NohoMsgType.PLAY_AUDIO;
    }

    public class PostOpCompleteMsg : BaseNohoMessage
    {
        public override MessageConstants.NohoMsgType NohoMsgType => MessageConstants.NohoMsgType.POST_OP_COMPLETE;
    }

    public class BandageCoveredMsg : BaseNohoMessage
    {
        public GameObject Bandage;
        public BandagePointController Point;
        public override MessageConstants.NohoMsgType NohoMsgType => MessageConstants.NohoMsgType.BANDAGE_COVERED;
    }

    public class BandagePointsCompleteMsg : BaseNohoMessage
    {
        public BandagePointsController Points;
        public override MessageConstants.NohoMsgType NohoMsgType => MessageConstants.NohoMsgType.BANDAGE_POINTS_COMPLETE;
    }

    public class LoadCharacterEpisodesMsg : BaseNohoMessage
    {
        public string CharacterDevName;
        public override MessageConstants.NohoMsgType NohoMsgType => MessageConstants.NohoMsgType.LOAD_CHARACTER_EPISODES;
    }
}