<?php
class Activity_model extends CI_Model {

    public $user;
    public $type;
    public $target;
    public $created_at;

    public function __construct() {
        parent::__construct();
    }

    public function get_all() {
		$query = $this->db->get('activity');
		return $query->result();
    }

    public function get_type_count_by_dates($type, $start_date, $end_date) {
        $this->db->where('type', $type);
        $this->db->where('created_at >', $start_date);
        $this->db->where('created_at <', $end_date);
		$query = $this->db->get('activity');
		return $query->num_rows();
    }

    public function get_type_count_all($type) {
        $this->db->where('type', $type);
		$query = $this->db->get('activity');
		return $query->num_rows();
    }

    public function get_type_count_all_for_target($type, $target) {
        $this->db->where('type', $type);
        $this->db->where('target', $target);
		$query = $this->db->get('activity');
		return $query->num_rows();
    }

    public function get_all_path_activation_count() {
        $this->db->where('type', 'path_activate');
		$query = $this->db->get('activity');
		return $query->num_rows();
    }

    public function get_all_node_completion_count() {
        $this->db->where('type', 'node_complete');
		$query = $this->db->get('activity');
		return $query->num_rows();
    }

    public function get_node_completion_count($nid) {
        $this->db->where('target', $nid);
        $this->db->where('type', 'node_complete');
		$query = $this->db->get('activity');
		return $query->num_rows();
    }

    public function get_path_activation_count($pid) {
        $this->db->where('target', $pid);
        $this->db->where('type', 'path_activate');
		$query = $this->db->get('activity');
		return $query->num_rows();
    }

    public function get_all_login_count() {
        $this->db->where('type', 'login');
		$query = $this->db->get('activity');
		return $query->num_rows();
    }

    public function get_user_login_count($uid) {
        $this->db->where('type', 'login');
        $this->db->where('user', $uid);
		$query = $this->db->get('activity');
		return $query->num_rows();
    }

    public function get_last_twenty() {
        $this->db->limit(20);
        $this->db->order_by('created_at', 'DESC');
		$query = $this->db->get('activity');
		return $query->result();
    }

    public function log() {
        $this->user = $this->input->post('user');
        $this->type = $this->input->post('type');
        $this->target = $this->input->post('target');
        $this->created_at = time();

        $this->db->insert('activity', $this);
    }

    public function log_manual($user, $type, $target, $created_at) {
        $this->user = $user;
        $this->type = $type;
        $this->target = $target;
        $this->created_at = $created_at;

        $this->db->insert('activity', $this);
    }


    public function update($id, $user, $type, $target, $created_at) {
        $this->user = $user;
        $this->type = $type;
        $this->target = $target;
        $this->created_at =$created_at;

        $this->db->update('activity', $this, array('id' => $id));
    }
}
?>
