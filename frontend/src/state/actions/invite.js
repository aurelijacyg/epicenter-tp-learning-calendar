import Axios from 'axios';
import makeSyncActionCreator from '../syncActionCreator';
import { INVITE_START, INVITE_SUCCESS, INVITE_FAIL } from './types';

const inviteStart = makeSyncActionCreator(INVITE_START);
const inviteSuccess = makeSyncActionCreator(INVITE_SUCCESS);
const inviteFail = makeSyncActionCreator(INVITE_FAIL);

const invite = ({
  email, firstName, lastName, role,
}) => async dispatch => {
  try {
    dispatch(inviteStart());
    await Axios.post('invites/invite', {
      email, firstName, lastName, role,
    });
    dispatch(inviteSuccess());
  } catch (err) {
    console.log(err);
    dispatch(inviteFail());
  }
};

export { invite };