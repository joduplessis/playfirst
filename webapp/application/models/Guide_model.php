<?php
class Guide_model extends CI_Model {

    public $text;
    public $node;

    public function __construct()
    {
        parent::__construct();
    }

    public function get_all() {
  		$query = $this->db->get('guides');
  		return $query->result();
    }

    public function get_one($id) {
  		$this->db->where('id', $id);
  		$query = $this->db->get('guides');
  		return $query->result();
    }

    public function update($data) {
        $this->db->where('id', $data["id"]);
        return $this->db->update('guides', $data);
    }

    public function delete($id) {
        $this->db->delete('guides', array('id' => $id));
    }

    public function add($text, $node) {
        $this->text = $text;
        $this->node = $node;
        $this->db->insert('guides', $this);
    }
}
?>
