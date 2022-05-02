<?php

class Path_model extends CI_Model {

    public $title;
    public $description;
    public $requirement;
    public $user;

    public function __construct() {
        parent::__construct();
    }

    public function get_all() {
        $query = $this->db->get('paths');
        return $query->result();
    }

    public function get_one($id) {
  		$this->db->where('id', $id);
  		$query = $this->db->get('paths');

  		return $query->result();
    }

    public function update() {
      $id                 = $this->input->post('id');
      $this->title        = $this->input->post('title');
      $this->user         = $this->input->post('user');
      $this->description  = $this->input->post('description');
      $this->requirement  = $this->input->post('requirement');

      $this->db->update('paths', $this, array('id' => $id));
    }

    public function delete($id) {
        $query = $this->db->delete('paths', array('id' => $id));
    }

    public function add($title) {
        $this->title        = $title;
        $this->description  = "Brand new path for the player";
        $this->requirement  = 0;
        $this->user         = $this->session->get_userdata("user")["user"][0]->id;

        $this->db->insert('paths', $this);
    }
}
?>
