<?php
class World_model extends CI_Model {

    public $title;
    public $start;

    public function __construct()
    {
        parent::__construct();
    }

    public function get_all() {
  		$query = $this->db->get('worlds');
  		return $query->result();
    }

    public function get_one($id) {
  		$this->db->where('id', $id);
  		$query = $this->db->get('worlds');
  		return $query->result();
    }

    public function update() {
        $id = $this->input->post('id');
        $start = 0;
        if (isset($_POST["start"])) { $start = 1; }
        $this->title = $this->input->post('title');
        $this->start = $start;
        $this->db->update('worlds', $this, array('id' => $id));
    }

    public function delete($id) {
        $this->db->delete('worlds', array('id' => $id));
    }

    public function add($title) {
        $this->title = $title;
        $this->start = 0;
        $this->db->insert('worlds', $this);
    }
}
?>
