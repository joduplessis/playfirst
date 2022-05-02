<?php
class User_model extends CI_Model {

    public $fullname;
    public $playname;
    public $email;
    public $password;
    public $points;
    public $role;
    public $avatar;
    public $badges;
    public $nodes;
    public $paths;

    public function __construct() {
        parent::__construct();
    }

    public function login() {
        $email = $this->input->post('email');
        $password = $this->input->post('password');

		$this->db->where('email', $email);
        $this->db->where('password', $password);

		$query = $this->db->get('users');
		return $query->result();
    }

    public function update() {
        $id = $this->input->post('id');

        $this->fullname = $this->input->post('fullname');
        $this->playname = $this->input->post('playname');
        $this->email = $this->input->post('email');
        $this->password = $this->input->post('password');
        $this->points = $this->input->post('points');
        $this->role = $this->input->post('role');
        $this->avatar = $this->input->post('avatar');
        $this->badges = $this->input->post('badges');
        $this->nodes = $this->input->post('nodes');
        $this->paths = $this->input->post('paths');

        $this->db->update('users', $this, array('id' => $id));
    }

    public function get_one($id) {
		$this->db->where('id', $id);
		$query = $this->db->get('users');
		return $query->result();
    }

    public function get_all() {
		$query = $this->db->get('users');
		return $query->result();
    }

    public function get_all_badges() {
    	$query = $this->db->select('badges')->get('users');
        return $query->result();
    }

    public function checkUser($email, $password) {
        $this->db->where('email', $email);
        $this->db->where('password', $password);
		$query = $this->db->get('users');

        if ($query->num_rows()==0) {
            return 0;
        } else {
            return $query->row()->id;
        }
    }

}
?>
